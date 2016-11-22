/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
/*
 * Author:		Troispoils
 * Date:		2016/11/21	
 * Directory: /scripts/quests/Hibernia/1a5
 *
 * Description:
 * Do this quest early to get the most out of the cloak, but don't give it away -
 * you might need it again later (hint). 
 *
 * The trip between Ludlow and Humberton is a lot faster if you run over the hill instead 
 * of going around it - just watch out for boulderlings.
 * 
 * Talk to Steward Willie to receieve this quest. The objective it fairly simple, leave
 * the village and hunt down a wolf pup. You'll find them right outside near the road to Camelot. 
 *
 * Although you can get the quest at level 1, the wolf will be yellow to you until level 2.
 * Take a friend along or hunt around Humberton a bit before trying the pup. 
 *
 * After killing the wolf, take the pelt back to Steward Willie, and you'll receive a Wolf Head Token.
 * Take this over the hill to Seamstress Lynnet in Ludlow, and she'll make you the Wolf Pelt Cloak.
 */

using System;
using System.Reflection;
using DOL.Database;
using DOL.Events;
using DOL.GS.PacketHandler;
using log4net;
/* I suggest you declare yourself some namespaces for your quests
 * Like: DOL.GS.Quests.Albion
 *       DOL.GS.Quests.Midgard
 *       DOL.GS.Quests.Hibernia
 * Also this is the name that will show up in the database as QuestName
 * so setting good values here will result in easier to read and cleaner
 * Database Code
 */

namespace DOL.GS.Quests.Hibernia
{
	/* The first thing we do, is to declare the class we create
	 * as Quest. To do this, we derive from the abstract class
	 * AbstractQuest
	 * 
	 * This quest for example will be stored in the database with
	 * the name: DOL.GS.Quests.Albion.TheDevilsintheDetails
	 */

	public class TheDevilsintheDetails : BaseQuest
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/* Declare the variables we need inside our quest.
		 * You can declare static variables here, which will be available in 
		 * ALL instance of your quest and should be initialized ONLY ONCE inside
		 * the OnScriptLoaded method.
		 * 
		 * Or declare nonstatic variables here which can be unique for each Player
		 * and change through the quest journey...
		 * 
		 * We store our two mobs as static variables, since we need them
		 */

		protected const string questTitle = "The Devil's in the Details";
		protected const int minimumLevel = 1;
		protected const int maximumLevel = 4;

		private static GameNPC sentinelMaitias = null;
		private static GameNPC sentinelMoya = null;

		private static ItemTemplate boxTrain = null;

		/* We need to define the constructors from the base class here, else there might be problems
		 * when loading this quest...
		 */
		public TheDevilsintheDetails() : base()
		{
		}

		public TheDevilsintheDetails(GamePlayer questingPlayer) : this(questingPlayer, 1)
		{
		}

		public TheDevilsintheDetails(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public TheDevilsintheDetails(GamePlayer questingPlayer, DBQuest dbQuest) : base(questingPlayer, dbQuest)
		{
		}


		/* The following method is called automatically when this quest class
		 * is loaded. You might notice that this method is the same as in standard
		 * game events. And yes, quests basically are game events for single players
		 * 
		 * To make this method automatically load, we have to declare it static
		 * and give it the [ScriptLoadedEvent] attribute. 
		 *
		 * Inside this method we initialize the quest. This is neccessary if we 
		 * want to set the quest hooks to the NPCs.
		 * 
		 * If you want, you can however add a quest to the player from ANY place
		 * inside your code, from events, from custom items, from anywhere you
		 * want. We will do it the standard way here ... and make Sir Quait wail
		 * a bit about the loss of his sword! 
		 */

		[ScriptLoadedEvent]
		public static void ScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			if (!ServerProperties.Properties.LOAD_QUESTS)
				return;
			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initializing ...");
			/* First thing we do in here is to search for the NPCs inside
			* the world who comes from the certain Realm. If we find a the players,
			* this means we don't have to create a new one.
			* 
			* NOTE: You can do anything you want in this method, you don't have
			* to search for NPC's ... you could create a custom item, place it
			* on the ground and if a player picks it up, he will get the quest!
			* Just examples, do anything you like and feel comfortable with :)
			*/

			#region defineNPCs

			GameNPC[] npcs = WorldMgr.GetNPCsByName("Sentinel Maitias", eRealm.Hibernia);

			/* Whops, if the npcs array length is 0 then no Sir Quait exists in
				* this users Mob Database, so we simply create one ;-)
				* else we take the existing one. And if more than one exist, we take
				* the first ...
				*/
			if (npcs.Length == 0)
			{
				sentinelMaitias = new GameGuard();
				sentinelMaitias.Model = 381;
				sentinelMaitias.Name = "Sentinel Maitias";
				if (log.IsWarnEnabled)
					log.Warn("Could not find " + sentinelMaitias.Name + ", creating him ...");
				sentinelMaitias.GuildName = "Part of " + questTitle;
				sentinelMaitias.Realm = eRealm.Hibernia;
				sentinelMaitias.CurrentRegionID = 200;
				sentinelMaitias.Size = 50;
				sentinelMaitias.Level = 51;
				sentinelMaitias.X = 348237;
				sentinelMaitias.Y = 493406;
				sentinelMaitias.Z = 5176;
				sentinelMaitias.Heading = 1695;
				sentinelMaitias.EquipmentTemplateID = "15bc1cc4-6c01-4cd1-a26c-8e20c9377114";

				//You don't have to store the created mob in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database
				if (SAVE_INTO_DATABASE)
					sentinelMaitias.SaveIntoDatabase();


				sentinelMaitias.AddToWorld();
			}
			else
				sentinelMaitias = npcs[0];

			/* Now we do the same for the Lynnet.
			*/
			npcs = WorldMgr.GetNPCsByName("Sentinel Moya", eRealm.Hibernia);
			if (npcs.Length == 0)
			{
				sentinelMoya = new GameGuard();
				sentinelMoya.Model = 378;
				sentinelMoya.Name = "Sentinel Moya";
				if (log.IsWarnEnabled)
					log.Warn("Could not find " + sentinelMoya.Name + ", creating ...");
				sentinelMoya.GuildName = "Part of " + questTitle;
				sentinelMoya.Realm = eRealm.Hibernia; //Needs to be none, else we can't kill him ;-)
				sentinelMoya.CurrentRegionID = 1;
				sentinelMoya.Size = 50;
				sentinelMoya.Level = 51;
				sentinelMoya.X = 341784;
				sentinelMoya.Y = 467817;
				sentinelMoya.Z = 5200;
				sentinelMoya.Heading = 3948;
				sentinelMoya.EquipmentTemplateID = "64356d42-041a-489d-9ca7-a738bdc81d5c";

				//You don't have to store the creted mob in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database
				if (SAVE_INTO_DATABASE)
					sentinelMoya.SaveIntoDatabase();

				sentinelMoya.AddToWorld();
			}
			else
				sentinelMoya = npcs[0];

			#endregion

			#region defineItems

			boxTrain = GameServer.Database.FindObjectByKey<ItemTemplate>("Crate_of_Training_Supplies");
			if (boxTrain == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Crate of Training Supplies, creating it ...");
				boxTrain = new ItemTemplate();
				boxTrain.Name = "Crate of Training Supplies";
				boxTrain.Level = 1;
				boxTrain.Weight = 0;
				boxTrain.Model = 602;
				boxTrain.Id_nb = "Crate_of_Training_Supplies";
				boxTrain.IsPickable = true;
				boxTrain.IsDropable = true;
				boxTrain.Color = 0;
				boxTrain.Quality = 0;
				boxTrain.Condition = 0;
				boxTrain.MaxCondition = 0;
				boxTrain.Durability = 1;
				boxTrain.MaxDurability = 1;


				//You don't have to store the created TheDevilsintheDetails in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database

				GameServer.Database.AddObject(boxTrain);
			}

			#endregion

			/* Now we add some hooks to the Sir Quait we found.
				* Actually, we want to know when a player interacts with him.
				* So, we hook the right-click (interact) and the whisper method
				* of Sir Quait and set the callback method to the "TalkToXXX"
				* method. This means, the "TalkToXXX" method is called whenever
				* a player right clicks on him or when he whispers to him.
				*/

			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.AddHandler(sentinelMaitias, GameLivingEvent.Interact, new DOLEventHandler(TalkTosentinelMaitias));
			GameEventMgr.AddHandler(sentinelMaitias, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkTosentinelMaitias));

			GameEventMgr.AddHandler(sentinelMoya, GameLivingEvent.Interact, new DOLEventHandler(TalkTosentinelMoya));
			GameEventMgr.AddHandler(sentinelMoya, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkTosentinelMoya));

			/* Now we bring to stewardWillie the possibility to give this quest to players */
			sentinelMaitias.AddQuestToGive(typeof(TheDevilsintheDetails));
			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		/* The following method is called automatically when this quest class
		 * is unloaded. 
		 * 
		 * Since we set hooks in the load method, it is good practice to remove
		 * those hooks again!
		 */

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			/* If sirQuait has not been initialized, then we don't have to remove any
			 * hooks from him ;-)
			 */
			if (sentinelMaitias == null)
				return;

			/* Removing hooks works just as adding them but instead of 
			 * AddHandler, we call RemoveHandler, the parameters stay the same
			 */

			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.RemoveHandler(sentinelMaitias, GameLivingEvent.Interact, new DOLEventHandler(TalkTosentinelMaitias));
			GameEventMgr.RemoveHandler(sentinelMaitias, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkTosentinelMaitias));

			GameEventMgr.RemoveHandler(sentinelMoya, GameLivingEvent.Interact, new DOLEventHandler(TalkTosentinelMoya));
			GameEventMgr.RemoveHandler(sentinelMoya, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkTosentinelMoya));

			/* Now we remove to stewardWillie the possibility to give this quest to players */
			sentinelMaitias.RemoveQuestToGive(typeof(TheDevilsintheDetails));
		}

		/* This is the method we declared as callback for the hooks we set to
		 * Sir Quait. It will be called whenever a player right clicks on Sir Quait
		 * or when he whispers something to him.
		 */

		protected static void TalkTosentinelMaitias(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs)args).Source as GamePlayer;
			if (player == null)
				return;

			if (sentinelMaitias.CanGiveQuest(typeof(TheDevilsintheDetails), player) <= 0)
				return;

			//We also check if the player is already doing the quest
			TheDevilsintheDetails quest = player.IsDoingQuest(typeof(TheDevilsintheDetails)) as TheDevilsintheDetails;

			sentinelMaitias.TurnTo(player);
			//Did the player rightclick on Sir Quait?
			if (e == GameObjectEvent.Interact)
			{
				//We check if the player is already doing the quest
				if (quest != null)
				{
					//If the player is already doing the quest, we ask if he found the fur!
					if (player.Inventory.GetFirstItemByID(boxTrain.Id_nb, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack) != null)
						sentinelMaitias.SayTo(player, "Excellent. This crate of supplies must be delivered to my counterpart in Ardee, Sentinel Moya. The town is just a bit to the north, along the road. Without these new training swords, Moya would have difficulty drilling her recruits.");
					else
						sentinelMaitias.SayTo(player, "I see that you've returned. Did Sentinel Moya receive the supplies in [time]?");
					return;
				}
				else
				{
					sentinelMaitias.SayTo(player, "Good day to you, Guardian. Forgive me if I appear distracted, but my duties are many these days, and my staff keeps shrinking. In times past, Mag Mell formed the center of a vibrant elven community in Hibernia. Fagan dreams of [recreating] those days.");
					return;
				}
			}
			// The player whispered to Sir Quait (clicked on the text inside the [])
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs)args;

				//We also check if the player is already doing the quest
				if (quest == null)
				{
					switch (wArgs.Text)
					{
						case "recreating":
							sentinelMaitias.SayTo(player, "I don't blame him for being ambitious. Fagan has lived through tumultuous times and wants the best for his people, but I think that sometimes he focuses too much on the big picture. As a result, it falls to me to keep Mag Mell running on a daily [basis].");
							break;
						case "basis":
							sentinelMaitias.SayTo(player, "There should be some around the area of this village, take a look near the road to Camelot. Kill any wolf pups you can find, and bring me its fur.");
							player.Out.SendQuestSubscribeCommand(sentinelMaitias, QuestMgr.GetIDForQuestType(typeof(TheDevilsintheDetails)), "Do you accept The Devil's in the Details quest?");
							break;
					}
				}
				else
				{
					switch (wArgs.Text)
					{
						case "abort":
							player.Out.SendCustomDialog("Do you really want to abort this quest, \nall items gained during quest will be lost?", new CustomDialogResponse(CheckPlayerAbortQuest));
							break;
						case "time":
							sentinelMaitias.SayTo(player, "Thank you for taking care of that so quickly. Please take this coin as a reward for your efforts and perhaps we can work together again in the future. Be well.");
							quest.FinishQuest();
							break;
					}
				}

			}
		}

		protected static void SubscribeQuest(DOLEvent e, object sender, EventArgs args)
		{
			QuestEventArgs qargs = args as QuestEventArgs;
			if (qargs == null)
				return;

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(TheDevilsintheDetails)))
				return;

			if (e == GamePlayerEvent.AcceptQuest)
			{
				CheckPlayerAcceptQuest(qargs.Player, 0x01);
			}
			else if (e == GamePlayerEvent.DeclineQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x00);
		}

		/* This is the method we declared as callback for the hooks we set to
		 * Sir Quait. It will be called whenever a player right clicks on Sir Quait
		 * or when he whispers something to him.
		 */

		protected static void TalkTosentinelMoya(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs)args).Source as GamePlayer;
			if (player == null)
				return;
			//We also check if the player is already doing the quest
			TheDevilsintheDetails quest = player.IsDoingQuest(typeof(TheDevilsintheDetails)) as TheDevilsintheDetails;

			sentinelMoya.TurnTo(player);
			//Did the player rightclick on Sir Quait?
			if (e == GameObjectEvent.Interact)
			{
				if (player.Inventory.GetFirstItemByID(boxTrain.Id_nb, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack) != null)
					sentinelMoya.SayTo(player, "Greetings, Guardian. What brings you to Ardee [today]?");
				else
					sentinelMoya.SayTo(player, "Hello, how are you?");
				return;
			}
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs)args;
				switch (wArgs.Text)
				{
					case "today":
						RemoveItem(sentinelMoya, player, boxTrain);
						sentinelMoya.SayTo(player, "Thank you so much! When you return to Mag Mell, please give Sentinel Maitias my thanks as well. I'm looking forward to training more of Hibernia's Sentinels. Take care!");
						quest.Step = 2;
						break;
				}
			}
		}

		/// <summary>
		/// This method checks if a player qualifies for this quest
		/// </summary>
		/// <returns>true if qualified, false if not</returns>
		public override bool CheckQuestQualification(GamePlayer player)
		{
			// if the player is already doing the quest his level is no longer of relevance
			if (player.IsDoingQuest(typeof(TheDevilsintheDetails)) != null)
				return true;

			// This checks below are only performed is player isn't doing quest already

			if (player.Level < minimumLevel || player.Level > maximumLevel)
				return false;

			return true;
		}


		/* This is our callback hook that will be called when the player clicks
		 * on any button in the quest offer dialog. We check if he accepts or
		 * declines here...
		 */

		private static void CheckPlayerAbortQuest(GamePlayer player, byte response)
		{
			TheDevilsintheDetails quest = player.IsDoingQuest(typeof(TheDevilsintheDetails)) as TheDevilsintheDetails;

			if (quest == null)
				return;

			if (response == 0x00)
			{
				SendSystemMessage(player, "Good, now go out there and finish your work!");
			}
			else
			{
				SendSystemMessage(player, "Aborting Quest " + questTitle + ". You can start over again if you want.");
				quest.AbortQuest();
			}
		}

		/* This is our callback hook that will be called when the player clicks
		 * on any button in the quest offer dialog. We check if he accepts or
		 * declines here...
		 */

		private static void CheckPlayerAcceptQuest(GamePlayer player, byte response)
		{
			//We recheck the qualification, because we don't talk to players
			//who are not doing the quest
			if (sentinelMaitias.CanGiveQuest(typeof(TheDevilsintheDetails), player) <= 0)
				return;

			if (player.IsDoingQuest(typeof(TheDevilsintheDetails)) != null)
				return;

			if (response == 0x00)
			{
				SendReply(player, "Oh well, if you change your mind, please come back!");
			}
			else
			{
				//Check if we can add the quest
				if (!sentinelMaitias.GiveQuest(typeof(TheDevilsintheDetails), player, 1))
					return;

				sentinelMaitias.SayTo(player, "Excellent. This crate of supplies must be delivered to my counterpart in Ardee, Sentinel Moya. The town is just a bit to the north, along the road. Without these new training swords, Moya would have difficulty drilling her recruits.");
				GiveItem(player, boxTrain);
			}
		}

		/* Now we set the quest name.
		 * If we don't override the base method, then the quest
		 * will have the name "UNDEFINED QUEST NAME" and we don't
		 * want that, do we? ;-)
		 */

		public override string Name
		{
			get { return questTitle; }
		}

		/* Now we set the quest step descriptions.
		 * If we don't override the base method, then the quest
		 * description for ALL steps will be "UNDEFINDED QUEST DESCRIPTION"
		 * and this isn't something nice either ;-)
		 */

		public override string Description
		{
			get
			{
				switch (Step)
				{
					case 1:
						return "This crate of supplies must be delivered to my counterpart in Ardee, Sentinel Moya.";
					case 2:
						return "When you return to Mag Mell, please give Sentinel Maitias my thanks as well.";
				}
				return base.Description;
			}
		}

		public override void Notify(DOLEvent e, object sender, EventArgs args)
		{
			GamePlayer player = sender as GamePlayer;

			if (player == null)
				return;
		}

		public override void AbortQuest()
		{
			base.AbortQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			RemoveItem(m_questPlayer, boxTrain, false);
		}

		public override void FinishQuest()
		{
			base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			//Give reward to player here ...

			m_questPlayer.GainExperience(GameLiving.eXPSource.Quest, 60, true);
			long money = Money.GetMoney(0, 0, 0, 0, 10 + Util.Random(50));
			m_questPlayer.AddMoney(money, "You recieve {0} for your service.");
			InventoryLogging.LogInventoryAction("(QUEST;" + Name + ")", m_questPlayer, eInventoryActionType.Quest, money);

		}

	}
}
