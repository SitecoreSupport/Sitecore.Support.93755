using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Sitecore.Support.Publishing.Pipelines.PublishItem
{
  public class DeleteOldBlob : PublishItemProcessor
  {
    public override void Process(PublishItemContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      Item targetItem = context.PublishHelper.GetTargetItem(context.ItemId);
      if (targetItem != null && context.Action == PublishAction.DeleteTargetItem)
      {
        foreach (Field field in targetItem.Fields)
        {
          if (field.IsBlobField && !string.IsNullOrEmpty(field.Value))
          {
            ItemManager.RemoveBlobStream(new Guid(field.Value), targetItem.Database);
          }
        }
      }
      else if ((targetItem != null) && (context.Action != PublishAction.None))
      {
        Item itemToPublish = context.PublishHelper.GetItemToPublish(context.ItemId);

        var versionToPublish = context.PublishHelper.GetVersionToPublish(itemToPublish);
        if (versionToPublish != null)
        {
          foreach (Field field2 in versionToPublish.Fields)
          {
            Field field3 = targetItem.Fields[field2.ID];
            if ((field3 != null) && field2.IsBlobField && (field2.Value != field3.Value) && !string.IsNullOrEmpty(field2.Value) && !string.IsNullOrEmpty(field3.Value))
            {
               ItemManager.RemoveBlobStream(new Guid(field3.Value), targetItem.Database);
            }
          }
        }       
      }
    }
  }
}
