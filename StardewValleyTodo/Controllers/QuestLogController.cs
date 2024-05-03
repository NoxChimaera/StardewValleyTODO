using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Quests;
using StardewValley.SpecialOrders;
using StardewValleyTodo.Game;
using StardewValleyTodo.Helpers;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo.Controllers {
    public class QuestLogController {
        private InventoryTracker _inventoryTracker;
        private Inventory _inventory;

        public QuestLogController(InventoryTracker inventoryTracker, Inventory inventory) {
            _inventoryTracker = inventoryTracker;
            _inventory = inventory;
        }

        public void ProcessInput(QuestLog menu) {
            var questPage = Reflect.GetPrivate<int>(menu, "questPage");

            if (questPage == -1) {
                // Quest is not selected
                return;
            }

            var quest = Reflect.GetPrivate<IQuest>(menu, "_shownQuest");
            TrackableQuest trackableQuest = null;

            switch (quest) {
                case FishingQuest fishingQuest:
                    trackableQuest = TrackFishingQuest(fishingQuest);
                    break;
                case ItemDeliveryQuest itemDeliveryQuest:
                    trackableQuest = TrackItemDeliveryQuest(itemDeliveryQuest);
                    break;
                case ItemHarvestQuest itemHarvestQuest:
                    trackableQuest = TrackItemHarvestQuest(itemHarvestQuest);
                    break;
                case LostItemQuest lostItemQuest:
                    trackableQuest = TrackLostItemQuest(lostItemQuest);
                    break;
                case ResourceCollectionQuest resourceCollectionQuest:
                    trackableQuest = TrackResourceCollectionQuest(resourceCollectionQuest);
                    break;
                case SlayMonsterQuest slayMonsterQuest:
                    trackableQuest = TrackSlayMonsterQuest(slayMonsterQuest);
                    break;
                case SpecialOrder specialOrder:
                    trackableQuest = TrackSpecialOrder(specialOrder);
                    break;
                case Quest basicQuest:
                    trackableQuest = TrackBasicQuest(basicQuest);
                    break;

                default:
                    throw new NotImplementedException($"Unsupported quest type: {quest.GetType().Name}");
            }

            if (trackableQuest != null) {
                _inventoryTracker.Toggle(trackableQuest);
            }
        }

        private TrackableQuest TrackSpecialOrder(SpecialOrder specialOrder) {
            var name = $"{specialOrder.requester.Value}: Special Order";

            var items = new List<TrackableItemBase>();
            foreach (var objective in specialOrder.objectives) {
                if (!objective.ShouldShowProgress()) {
                    continue;
                }

                var item = new TrackableDynamicItem(objective.GetDescription(), () => objective.GetMaxCount(),
                    () => objective.GetCount());
                items.Add(item);
            }

            return new TrackableQuest(name, items, specialOrder);
        }

        private TrackableQuest TrackCraftingQuest(CraftingQuest quest) {
            var name = quest.questTitle;

            TrackableItemBase questItem = null;
            var key = ObjectKey.Parse(quest.ItemId.Value);

            if (key.Contains("-")) {
                throw new NotImplementedException("Can not track categories");
            } else {
                var info = Game1.objectData[key];
                var itemName = LocalizedStringLoader.Load(info.DisplayName);

                questItem = (new CountableItem(key, itemName, 1));
            }

            return new TrackableQuest(name, questItem, quest);
        }

        private TrackableQuest TrackItemDeliveryQuest(ItemDeliveryQuest quest) {
            var name = $"{quest.target.Value}: {quest.questTitle}";

            TrackableItemBase questItem = null;
            var key = ObjectKey.Parse(quest.ItemId.Value);

            if (key.Contains("-")) {
                throw new NotImplementedException("Can not track categories");
            } else {
                var info = Game1.objectData[key];
                var itemName = LocalizedStringLoader.Load(info.DisplayName);

                questItem = (new CountableItem(key, itemName, quest.number.Value));
            }

            return new TrackableQuest(name, questItem, quest);
        }

        private TrackableQuest TrackFishingQuest(FishingQuest quest) {
            var name = $"{quest.target.Value}: {quest.questTitle}";

            TrackableItemBase questItem = null;
            var key = ObjectKey.Parse(quest.ItemId.Value);

            if (key.Contains("-")) {
                throw new NotImplementedException("Can not track categories");
            } else {
                var info = Game1.objectData[key];
                var itemName = LocalizedStringLoader.Load(info.DisplayName);

                questItem = (new CountableItem(key, itemName, quest.numberToFish.Value));
            }

            return new TrackableQuest(name, questItem, quest);
        }

        private TrackableQuest TrackItemHarvestQuest(ItemHarvestQuest quest) {
            var name = quest.questTitle;

            TrackableItemBase questItem = null;
            var key = ObjectKey.Parse(quest.ItemId.Value);

            if (key.Contains("-")) {
                throw new NotImplementedException("Can not track categories");
            } else {
                var info = Game1.objectData[key];
                var itemName = LocalizedStringLoader.Load(info.DisplayName);

                questItem = (new CountableItem(key, itemName, quest.Number.Value));
            }

            return new TrackableQuest(name, questItem, quest);
        }

        private TrackableQuest TrackLostItemQuest(LostItemQuest quest) {
            var name = $"{quest.npcName}: {quest.questTitle}";

            TrackableItemBase questItem = null;
            var key = ObjectKey.Parse(quest.ItemId.Value);

            if (key.Contains("-")) {
                throw new NotImplementedException("Can not track categories");
            } else {
                var info = Game1.objectData[key];
                var itemName = LocalizedStringLoader.Load(info.DisplayName);

                questItem = (new CountableItem(key, itemName, 1));
            }

            return new TrackableQuest(name, questItem, quest);
        }

        private TrackableQuest TrackResourceCollectionQuest(ResourceCollectionQuest quest) {
            var name = $"{quest.target.Value}: {quest.questTitle}";

            TrackableItemBase questItem = null;
            var key = ObjectKey.Parse(quest.ItemId.Value);

            if (key.Contains("-")) {
                throw new NotImplementedException("Can not track categories");
            } else {
                var info = Game1.objectData[key];
                var itemName = LocalizedStringLoader.Load(info.DisplayName);

                questItem = (new CountableItem(key, itemName, quest.number.Value));
            }

            return new TrackableQuest(name, questItem, quest);
        }

        private TrackableQuest TrackSlayMonsterQuest(SlayMonsterQuest quest) {
            var name = $"{quest.target.Value}: {quest.questTitle}";
            TrackableItemBase questItem = new TrackableDynamicItem(quest.monsterName.Value,
                () => quest.numberToKill.Value, () => quest.numberKilled.Value);
            return new TrackableQuest(name, questItem, quest);
        }

        private TrackableQuest TrackBasicQuest(Quest quest) {
            var name = quest.questTitle;
            TrackableItemBase questItem =
                new TrackableDynamicItem(quest.currentObjective, () => 1, () => quest.completed.Value ? 1 : 0, false);
            return new TrackableQuest(name, questItem, quest);
        }
    }
}
