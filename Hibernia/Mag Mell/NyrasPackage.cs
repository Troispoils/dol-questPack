
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
* Author:	Troispoils
* Date:		2016-11-20
*
* Notes:
*  DOL Server example quest (Nyra's Package) rewritten using the Quest Designer
*/

using System;
using System.Reflection;
using DOL.Database;
using DOL.Events;
using DOL.GS.PacketHandler;
using log4net;
using DOL.AI.Brain;

namespace DOL.GS.Quests.Hibernia {
	
     /* The first thing we do, is to declare the class we create
	 * as Quest. To do this, we derive from the abstract class
	 * BaseQuest	  	 
	 */
	public class NyrasPackage : BaseQuest
	{
		/// <summary>
		/// Defines a logger for this class.
		///
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
		*/

		protected const string questTitle = "Nyra's Package";

		protected const int minimumLevel = 1;
		protected const int maximumLevel = 5;
	
	
		private static GameNPC Nyra = null;
		
		private static GameNPC Taran = null;
		
		private static ItemTemplate LettertoNyra = null;

        private static ItemTemplate SmallBoxforTaran = null;

        private static ItemTemplate SilverRingofHealth = null;


        // Custom Initialization Code Begin

        // Custom Initialization Code End

        /* 
		* Constructor
		*/
        public NyrasPackage() : base()
		{
		}

		public NyrasPackage(GamePlayer questingPlayer) : this(questingPlayer, 1)
		{
		}

		public NyrasPackage(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public NyrasPackage(GamePlayer questingPlayer, DBQuest dbQuest) : base(questingPlayer, dbQuest)
		{
		}

	[ScriptLoadedEvent]
	public static void ScriptLoaded(DOLEvent e, object sender, EventArgs args)
	{
	    if (!ServerProperties.Properties.LOAD_QUESTS)
		    return;
	    if (log.IsInfoEnabled)
		    log.Info("Quest \"" + questTitle + "\" initializing ...");

		    #region defineNPCs
	    GameNPC[] npcsNyra;
		GameNPC[] npcsTaran;
	
		npcsNyra = WorldMgr.GetNPCsByName("Nyra",(eRealm) 3);
		if (npcsNyra.Length == 0)
		{			
			Nyra = new DOL.GS.GameNPC();
                Nyra.Model = 377;
                Nyra.Name = "Nyra";
			if (log.IsWarnEnabled)
				log.Warn("Could not find " + Nyra.Name + ", creating ...");
                //Nyra.GuildName = "Part of " + questTitle + " Quest";
                Nyra.Realm = eRealm.Hibernia;
                Nyra.CurrentRegionID = 200;
                Nyra.Size = 50;
                Nyra.Level = 10;
                Nyra.MaxSpeedBase = 200;
                Nyra.Faction = FactionMgr.GetFactionByID(0);
                Nyra.X = 347415;
                Nyra.Y = 492588;
                Nyra.Z = 5199;
                Nyra.Heading = 523;
                Nyra.RespawnInterval = 0;
                Nyra.BodyType = 0;
			

			StandardMobBrain brain = new StandardMobBrain();
			brain.AggroLevel = 0;
			brain.AggroRange = 0;
                Nyra.SetOwnBrain(brain);
			
			//You don't have to store the created mob in the db if you don't want,
			//it will be recreated each time it is not found, just comment the following
			//line if you rather not modify your database
			if (SAVE_INTO_DATABASE)
                    Nyra.SaveIntoDatabase();

                Nyra.AddToWorld();
			
		}
		else 
		{
                Nyra = npcsNyra[0];
		}

		npcsTaran = WorldMgr.GetNPCsByName("Taran",(eRealm) 3);
		if (npcsTaran.Length == 0)
		{			
			Taran = new DOL.GS.GameNPC();
                Taran.Model = 350;
                Taran.Name = "Taran";
			if (log.IsWarnEnabled)
				log.Warn("Could not find " + Taran.Name + ", creating ...");
                Taran.GuildName = "Smith";
                Taran.Realm = eRealm.Hibernia;
                Taran.CurrentRegionID = 200;
                Taran.Size = 50;
                Taran.Level = 35;
                Taran.MaxSpeedBase = 200;
                Taran.Faction = FactionMgr.GetFactionByID(0);
                Taran.X = 336639;
                Taran.Y = 484496;
                Taran.Z = 5200;
                Taran.Heading = 297;
                Taran.RespawnInterval = 0;
                Taran.BodyType = 0;
			

			StandardMobBrain brain = new StandardMobBrain();
			brain.AggroLevel = 0;
			brain.AggroRange = 0;
                Taran.SetOwnBrain(brain);
			
			//You don't have to store the created mob in the db if you don't want,
			//it will be recreated each time it is not found, just comment the following
			//line if you rather not modify your database
			if (SAVE_INTO_DATABASE)
                    Taran.SaveIntoDatabase();

                Taran.AddToWorld();
			
		}
		else 
		{
                Taran = npcsTaran[0];
		}


            #endregion

            #region defineItems

            LettertoNyra = GameServer.Database.FindObjectByKey<ItemTemplate>("Letter_to_Nyra");
		if (LettertoNyra == null)
		{
                LettertoNyra = new ItemTemplate();
                LettertoNyra.Name = "Letter to Nyra";
			if (log.IsWarnEnabled)
				log.Warn("Could not find " + LettertoNyra.Name + ", creating it ...");
                LettertoNyra.Level = 1;
                LettertoNyra.Weight = 0;
                LettertoNyra.Model = 499;
                LettertoNyra.Object_Type = 0;
                LettertoNyra.Item_Type = 1;
                LettertoNyra.Id_nb = "Letter_to_Nyra";
                LettertoNyra.Hand = 0;
                LettertoNyra.Price = 0;
                LettertoNyra.IsPickable = true;
                LettertoNyra.IsDropable = true;
                LettertoNyra.IsTradable = false;
                LettertoNyra.CanDropAsLoot = false;
                LettertoNyra.Color = 0;
                LettertoNyra.Bonus = 0; // default bonus				
                LettertoNyra.Bonus1 = 0;
                LettertoNyra.Bonus1Type = (int) 0;
			    LettertoNyra.Bonus2 = 0;
			    LettertoNyra.Bonus2Type = (int) 0;
			    LettertoNyra.Bonus3 = 0;
			    LettertoNyra.Bonus3Type = (int) 0;
			    LettertoNyra.Bonus4 = 0;
			    LettertoNyra.Bonus4Type = (int) 0;
			    LettertoNyra.Bonus5 = 0;
			    LettertoNyra.Bonus5Type = (int) 0;
			    LettertoNyra.Bonus6 = 0;
			    LettertoNyra.Bonus6Type = (int) 0;
			    LettertoNyra.Bonus7 = 0;
			    LettertoNyra.Bonus7Type = (int) 0;
			    LettertoNyra.Bonus8 = 0;
			    LettertoNyra.Bonus8Type = (int) 0;
			    LettertoNyra.Bonus9 = 0;
			    LettertoNyra.Bonus9Type = (int) 0;
			    LettertoNyra.Bonus10 = 0;
			    LettertoNyra.Bonus10Type = (int) 0;
			    LettertoNyra.ExtraBonus = 0;
			    LettertoNyra.ExtraBonusType = (int) 0;
			    LettertoNyra.Effect = 0;
			    LettertoNyra.Emblem = 0;
			    LettertoNyra.Charges = 0;
			    LettertoNyra.MaxCharges = 0;
			    LettertoNyra.SpellID = 0;
			    LettertoNyra.ProcSpellID = 0;
			    LettertoNyra.Type_Damage = 0;
			    LettertoNyra.Realm = 0;
			    LettertoNyra.MaxCount = 1;
			    LettertoNyra.PackSize = 1;
			    LettertoNyra.Extension = 0;
			    LettertoNyra.Quality = 1;				
			    LettertoNyra.Condition = 1;
			    LettertoNyra.MaxCondition = 1;
			    LettertoNyra.Durability = 1;
			    LettertoNyra.MaxDurability = 1;
			    LettertoNyra.PoisonCharges = 0;
			    LettertoNyra.PoisonMaxCharges = 0;
			    LettertoNyra.PoisonSpellID = 0;
			    LettertoNyra.ProcSpellID1 = 0;
			    LettertoNyra.SpellID1 = 0;
			    LettertoNyra.MaxCharges1 = 0;
			    LettertoNyra.Charges1 = 0;
			
			//You don't have to store the created item in the db if you don't want,
			//it will be recreated each time it is not found, just comment the following
			//line if you rather not modify your database
			
				GameServer.Database.AddObject(LettertoNyra);
			}

				SmallBoxforTaran = GameServer.Database.FindObjectByKey<ItemTemplate>("Small_Box_for_Taran");
			if (SmallBoxforTaran == null)
			{
				SmallBoxforTaran = new ItemTemplate();
				SmallBoxforTaran.Name = "Small Box for Taran";
				if (log.IsWarnEnabled)
					log.Warn("Could not find " + SmallBoxforTaran.Name + ", creating it ...");
				SmallBoxforTaran.Level = 1;
				SmallBoxforTaran.Weight = 0;
				SmallBoxforTaran.Model = 602;
				SmallBoxforTaran.Object_Type = 0;
				SmallBoxforTaran.Item_Type = 1;
				SmallBoxforTaran.Id_nb = "Small_Box_for_Taran";
				SmallBoxforTaran.Hand = 0;
				SmallBoxforTaran.Price = 0;
				SmallBoxforTaran.IsPickable = true;
				SmallBoxforTaran.IsDropable = true;
				SmallBoxforTaran.IsTradable = false;
				SmallBoxforTaran.CanDropAsLoot = false;
				SmallBoxforTaran.Color = 0;
				SmallBoxforTaran.Bonus = 0; // default bonus				
				SmallBoxforTaran.Bonus1 = 0;
				SmallBoxforTaran.Bonus1Type = (int)0;
				SmallBoxforTaran.Bonus2 = 0;
				SmallBoxforTaran.Bonus2Type = (int)0;
				SmallBoxforTaran.Bonus3 = 0;
				SmallBoxforTaran.Bonus3Type = (int)0;
				SmallBoxforTaran.Bonus4 = 0;
				SmallBoxforTaran.Bonus4Type = (int)0;
				SmallBoxforTaran.Bonus5 = 0;
				SmallBoxforTaran.Bonus5Type = (int)0;
				SmallBoxforTaran.Bonus6 = 0;
				SmallBoxforTaran.Bonus6Type = (int)0;
				SmallBoxforTaran.Bonus7 = 0;
				SmallBoxforTaran.Bonus7Type = (int)0;
				SmallBoxforTaran.Bonus8 = 0;
				SmallBoxforTaran.Bonus8Type = (int)0;
				SmallBoxforTaran.Bonus9 = 0;
				SmallBoxforTaran.Bonus9Type = (int)0;
				SmallBoxforTaran.Bonus10 = 0;
				SmallBoxforTaran.Bonus10Type = (int)0;
				SmallBoxforTaran.ExtraBonus = 0;
				SmallBoxforTaran.ExtraBonusType = (int)0;
				SmallBoxforTaran.Effect = 0;
				SmallBoxforTaran.Emblem = 0;
				SmallBoxforTaran.Charges = 0;
				SmallBoxforTaran.MaxCharges = 0;
				SmallBoxforTaran.SpellID = 0;
				SmallBoxforTaran.ProcSpellID = 0;
				SmallBoxforTaran.Type_Damage = 0;
				SmallBoxforTaran.Realm = 0;
				SmallBoxforTaran.MaxCount = 1;
				SmallBoxforTaran.PackSize = 1;
				SmallBoxforTaran.Extension = 0;
				SmallBoxforTaran.Quality = 1;
				SmallBoxforTaran.Condition = 1;
				SmallBoxforTaran.MaxCondition = 1;
				SmallBoxforTaran.Durability = 1;
				SmallBoxforTaran.MaxDurability = 1;
				SmallBoxforTaran.PoisonCharges = 0;
				SmallBoxforTaran.PoisonMaxCharges = 0;
				SmallBoxforTaran.PoisonSpellID = 0;
				SmallBoxforTaran.ProcSpellID1 = 0;
				SmallBoxforTaran.SpellID1 = 0;
				SmallBoxforTaran.MaxCharges1 = 0;
				SmallBoxforTaran.Charges1 = 0;

				//You don't have to store the created item in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database

				GameServer.Database.AddObject(SmallBoxforTaran);
			}

				SilverRingofHealth = GameServer.Database.FindObjectByKey<ItemTemplate>("Silver_Ring_of_Health");
			if (SilverRingofHealth == null)
			{
				SilverRingofHealth = new ItemTemplate();
				SilverRingofHealth.Name = "Silver Ring of Health";
				if (log.IsWarnEnabled)
					log.Warn("Could not find " + SilverRingofHealth.Name + ", creating it ...");
				SilverRingofHealth.Level = 3;
				SilverRingofHealth.Weight = 0;
				SilverRingofHealth.Model = 103;
				SilverRingofHealth.Object_Type = 41;
				SilverRingofHealth.Item_Type = 35;
				SilverRingofHealth.Id_nb = "Silver_Ring_of_Health_hib";
				SilverRingofHealth.Hand = 0;
				SilverRingofHealth.Price = 0;
				SilverRingofHealth.IsPickable = true;
				SilverRingofHealth.IsDropable = true;
				SilverRingofHealth.IsTradable = true;
				SilverRingofHealth.CanDropAsLoot = false;
				SilverRingofHealth.Color = 0;
				SilverRingofHealth.Bonus = 0; // default bonus				
				SilverRingofHealth.Bonus1 = 3;
				SilverRingofHealth.Bonus1Type = (int)8;
				SilverRingofHealth.Bonus2 = 17;
				SilverRingofHealth.Bonus2Type = (int)1;
				SilverRingofHealth.Bonus3 = 0;
				SilverRingofHealth.Bonus3Type = (int)0;
				SilverRingofHealth.Bonus4 = 0;
				SilverRingofHealth.Bonus4Type = (int)0;
				SilverRingofHealth.Bonus5 = 0;
				SilverRingofHealth.Bonus5Type = (int)0;
				SilverRingofHealth.Bonus6 = 0;
				SilverRingofHealth.Bonus6Type = (int)0;
				SilverRingofHealth.Bonus7 = 0;
				SilverRingofHealth.Bonus7Type = (int)0;
				SilverRingofHealth.Bonus8 = 0;
				SilverRingofHealth.Bonus8Type = (int)0;
				SilverRingofHealth.Bonus9 = 0;
				SilverRingofHealth.Bonus9Type = (int)0;
				SilverRingofHealth.Bonus10 = 0;
				SilverRingofHealth.Bonus10Type = (int)0;
				SilverRingofHealth.ExtraBonus = 0;
				SilverRingofHealth.ExtraBonusType = (int)0;
				SilverRingofHealth.Effect = 0;
				SilverRingofHealth.Emblem = 0;
				SilverRingofHealth.Charges = 0;
				SilverRingofHealth.MaxCharges = 0;
				SilverRingofHealth.SpellID = 0;
				SilverRingofHealth.ProcSpellID = 0;
				SilverRingofHealth.Type_Damage = 0;
				SilverRingofHealth.Realm = 3;
				SilverRingofHealth.MaxCount = 1;
				SilverRingofHealth.PackSize = 1;
				SilverRingofHealth.Extension = 0;
				SilverRingofHealth.Quality = 1;
				SilverRingofHealth.Condition = 1;
				SilverRingofHealth.MaxCondition = 1;
				SilverRingofHealth.Durability = 1;
				SilverRingofHealth.MaxDurability = 1;
				SilverRingofHealth.PoisonCharges = 0;
				SilverRingofHealth.PoisonMaxCharges = 0;
				SilverRingofHealth.PoisonSpellID = 0;
				SilverRingofHealth.ProcSpellID1 = 0;
				SilverRingofHealth.SpellID1 = 0;
				SilverRingofHealth.MaxCharges1 = 0;
				SilverRingofHealth.Charges1 = 0;

				//You don't have to store the created item in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database

				GameServer.Database.AddObject(SilverRingofHealth);
			}

			#endregion


			// Custom Scriptloaded Code Begin
			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.AddHandler(Nyra, GameLivingEvent.Interact, new DOLEventHandler(TalkToNyra));
			GameEventMgr.AddHandler(Nyra, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToNyra));

			GameEventMgr.AddHandler(Taran, GameLivingEvent.Interact, new DOLEventHandler(TalkToTaran));
			GameEventMgr.AddHandler(Taran, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToTaran));

			// Custom Scriptloaded Code End

			Nyra.AddQuestToGive(typeof (NyrasPackage));
			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{			
			if (Nyra == null)
				return;

			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.RemoveHandler(Nyra, GameLivingEvent.Interact, new DOLEventHandler(TalkToNyra));
			GameEventMgr.RemoveHandler(Nyra, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToNyra));

			GameEventMgr.RemoveHandler(Taran, GameLivingEvent.Interact, new DOLEventHandler(TalkToTaran));
			GameEventMgr.RemoveHandler(Taran, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToTaran));

			/* Now we remove the possibility to give this quest to players */
			Nyra.RemoveQuestToGive(typeof (NyrasPackage));
		}

		protected static void TalkToNyra(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs)args).Source as GamePlayer;
			if (player == null)
				return;

			if (Nyra.CanGiveQuest(typeof(NyrasPackage), player) <= 0)
				return;

			//We also check if the player is already doing the quest
			NyrasPackage quest = player.IsDoingQuest(typeof(NyrasPackage)) as NyrasPackage;

			Nyra.TurnTo(player);
			//Did the player rightclick on Sir Quait?
			if (e == GameObjectEvent.Interact)
			{
				//We check if the player is already doing the quest
				if (quest != null)
				{
					//If the player is already doing the quest, we ask if he found the fur!
					if (player.Inventory.GetFirstItemByID(SmallBoxforTaran.Id_nb, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack) != null)
						Nyra.SayTo(player, "Welcome back friend! I take it you didn't have any problems finding him? Did he have anything to say?");
					else if(player.Inventory.GetFirstItemByID(LettertoNyra.Id_nb, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack) != null)
						Nyra.SayTo(player, "Welcome back friend! I take it you didn't have any problems finding him? Did he have anything to say?");
					else if(quest.Step == 4)
						Nyra.SayTo(player, "Excellent! I was hoping he would like it. It was a small box of oranges. I know he loves them, and my parents have a small orange tree grove. Well, I think it's time I gave you [something].");
					return;
				}
				else
				{
					Nyra.SayTo(player, "Hi there! You seem new to Mag Mell. I guess it's lucky that you ran into me! I have something I need done, and you look like the person to do this [errand] for me!");
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
						case "errand":
							Nyra.SayTo(player, "Oh yes. See, I have something that needs to be delivered to Taran the Smith over by Tir na Nog. Do you think you'd be [able] to do that for me?");
							break;
						case "able":
							player.Out.SendQuestSubscribeCommand(Nyra, QuestMgr.GetIDForQuestType(typeof(NyrasPackage)), "Will you deliver Nyra's package to Taran the smith? [Level 1]");
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
						case "something":
							Nyra.SayTo(player, "Here you are. This is a ring I used a very long time ago, but I have no need for it now. I think it will suit you better. Thank you again my friend. Good journeys to you.");
							quest.FinishQuest();
							break;
					}
				}

			}
		}

		protected static void TalkToTaran(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs)args).Source as GamePlayer;
			if (player == null)
				return;
			//We also check if the player is already doing the quest
			NyrasPackage quest = player.IsDoingQuest(typeof(NyrasPackage)) as NyrasPackage;

			Taran.TurnTo(player);
			//Did the player rightclick on Sir Quait?
			if (e == GameObjectEvent.Interact)
			{
				if (player.Inventory.GetFirstItemByID(SmallBoxforTaran.Id_nb, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack) != null)
					Taran.SayTo(player, "Welcome friend. What can I do for you today?");
				else if(quest.Step == 2)
					Taran.SayTo(player, "Ah, the box I've been waiting for from Nyra. Thank you friend. If you'll wait just one second, I have a letter I'd like for you to take back to [her].");
				return;
			}
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs)args;
				switch (wArgs.Text)
				{
					case "her":
						Taran.SayTo(player, "Here you are, and a few silvers for your troubles. Good luck friend, and than you for the delivery.");
						quest.Step = 3;
						GiveItem(player, LettertoNyra, false);
						player.GainExperience(GameLiving.eXPSource.Quest, 10, true);
						break;
				}
			}
		}

		protected static void SubscribeQuest(DOLEvent e, object sender, EventArgs args)
		{
			QuestEventArgs qargs = args as QuestEventArgs;
			if (qargs == null)
				return;

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(NyrasPackage)))
				return;

			if (e == GamePlayerEvent.AcceptQuest)
			{
				CheckPlayerAcceptQuest(qargs.Player, 0x01);
			}
			else if (e == GamePlayerEvent.DeclineQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x00);
		}

		public override bool CheckQuestQualification(GamePlayer player)
		{
			// if the player is already doing the quest his level is no longer of relevance
			if (player.IsDoingQuest(typeof(NyrasPackage)) != null)
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
			NyrasPackage quest = player.IsDoingQuest(typeof(NyrasPackage)) as NyrasPackage;

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
			if (Nyra.CanGiveQuest(typeof(NyrasPackage), player) <= 0)
				return;

			if (player.IsDoingQuest(typeof(NyrasPackage)) != null)
				return;

			if (response == 0x00)
			{
				SendReply(player, "Oh well, if you change your mind, please come back!");
			}
			else
			{
				//Check if we can add the quest
				if (!Nyra.GiveQuest(typeof(NyrasPackage), player, 1))
					return;

				Nyra.SayTo(player, "Oh great! I knew you looked like the type I could count on! Ok, listen, just take this to Taran. He'll know what it's for.");
				GiveItem(player, SmallBoxforTaran);
			}
		}

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
						return "Deliver the package Nyra gave you to Taran the Smith near the gate to Tir na Nog.";
				
					case 2:
						return "Listen to Taran.";

                    case 3:
                        return "Deliver Taran's letter back to Nyra in Mag Mell.";

                    case 4:
                        return "Listen to Nyra. If she stops speaking with you, ask her is she has [something] for you for your hard work.";

                    default:
						return " No Queststep Description available.";
				}				
			}
		}

		/// <summary>
		/// This method checks if a player is qualified for this quest
		/// </summary>
		/// <returns>true if qualified, false if not</returns>
		public override void Notify(DOLEvent e, object sender, EventArgs args)
		{
			GamePlayer player = sender as GamePlayer;

			if (player == null)
				return;

			if (e == GamePlayerEvent.GiveItem)
			{
				GiveItemEventArgs gArgs = (GiveItemEventArgs)args;
				if (gArgs.Target.Name == Taran.Name && gArgs.Item.Id_nb == SmallBoxforTaran.Id_nb)
				{
					Taran.SayTo(player, "Ah, the box I've been waiting for from Nyra. Thank you friend. If you'll wait just one second, I have a letter I'd like for you to take back to [her].");
					RemoveItem(Taran, m_questPlayer, SmallBoxforTaran);
					Step = 2;
					return;
				}
				else if (gArgs.Target.Name == Nyra.Name && gArgs.Item.Id_nb == LettertoNyra.Id_nb)
				{
					Nyra.SayTo(player, "Excellent! I was hoping he would like it. It was a small box of oranges. I know he loves them, and my parents have a small orange tree grove. Well, I think it's time I gave you [something].");
					RemoveItem(Nyra, m_questPlayer, LettertoNyra);
					Step = 4;
					return;
				}
			}
		}

		public override void AbortQuest()
		{
			base.AbortQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			RemoveItem(m_questPlayer, SmallBoxforTaran, false);
			RemoveItem(m_questPlayer, LettertoNyra, false);
		}

		public override void FinishQuest()
		{
			base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			//Give reward to player here ...

			m_questPlayer.GainExperience(GameLiving.eXPSource.Quest, 40, true);
			long money = Money.GetMoney(0, 0, 0, 0, 10 + Util.Random(50));
			m_questPlayer.AddMoney(money, "You recieve {0} for your service.");
			GiveItem(m_questPlayer, SilverRingofHealth, false);
			InventoryLogging.LogInventoryAction("(QUEST;" + Name + ")", m_questPlayer, eInventoryActionType.Quest, money);

		}
	}
}
