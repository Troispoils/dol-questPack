
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

namespace DOL.GS.Quests.Hibernia
{

	/* The first thing we do, is to declare the class we create
	* as Quest. To do this, we derive from the abstract class
	* BaseQuest	  	 
	*/
	public class ChangelingTimes : BaseQuest
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

		protected const string questTitle = "Changeling Times";

		protected const int minimumLevel = 1;
		protected const int maximumLevel = 5;


		private static GameNPC Epona = null;

		private static ItemTemplate RecruitsRoundShield = null;

		private static ItemTemplate RecruitsSilveredBracelet = null;


		// Custom Initialization Code Begin

		// Custom Initialization Code End

		/* 
		* Constructor
		*/
		public ChangelingTimes() : base()
		{
		}

		public ChangelingTimes(GamePlayer questingPlayer) : this(questingPlayer, 1)
		{
		}

		public ChangelingTimes(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public ChangelingTimes(GamePlayer questingPlayer, DBQuest dbQuest) : base(questingPlayer, dbQuest)
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
			GameNPC[] npcsEpona;

			npcsEpona = WorldMgr.GetNPCsByName("Epona", (eRealm)3);
			if (npcsEpona.Length == 0)
			{
				Epona = new DOL.GS.GameHealer();
				Epona.Model = 386;
				Epona.Name = "Epona";
				if (log.IsWarnEnabled)
					log.Warn("Could not find " + Epona.Name + ", creating ...");
				//Nyra.GuildName = "Part of " + questTitle + " Quest";
				Epona.Realm = eRealm.Hibernia;
				Epona.CurrentRegionID = 200;
				Epona.Size = 50;
				Epona.Level = 38;
				Epona.MaxSpeedBase = 191;
				Epona.Faction = FactionMgr.GetFactionByID(0);
				Epona.X = 347606;
				Epona.Y = 490658;
				Epona.Z = 5220;
				Epona.Heading = 1342;
				Epona.RespawnInterval = 0;
				Epona.BodyType = 0;
				Epona.EquipmentTemplateID = "02fd82a3-46a3-44d8-8a22-b3a264acdad7";


				StandardMobBrain brain = new StandardMobBrain();
				brain.AggroLevel = 0;
				brain.AggroRange = 0;
				Epona.SetOwnBrain(brain);

				//You don't have to store the created mob in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database
				if (SAVE_INTO_DATABASE)
					Epona.SaveIntoDatabase();

				Epona.AddToWorld();

			}
			else
			{
				Epona = npcsEpona[0];
			}

			#endregion

			#region defineItems

			RecruitsRoundShield = GameServer.Database.FindObjectByKey<ItemTemplate>("Recruits_Round_Shield");
			if (RecruitsRoundShield == null)
			{
				RecruitsRoundShield = new ItemTemplate();
				RecruitsRoundShield.Name = "Recruit's Round Shield";
				if (log.IsWarnEnabled)
					log.Warn("Could not find " + RecruitsRoundShield.Name + ", creating it ...");
				RecruitsRoundShield.Level = 4;
				RecruitsRoundShield.Weight = 3;
				RecruitsRoundShield.Model = 59;
				RecruitsRoundShield.Object_Type = 42;
				RecruitsRoundShield.Item_Type = 11;
				RecruitsRoundShield.Id_nb = "Recruits_Round_Shield";
				RecruitsRoundShield.Hand = 0;
				RecruitsRoundShield.Price = 3;
				RecruitsRoundShield.IsPickable = true;
				RecruitsRoundShield.IsDropable = true;
				RecruitsRoundShield.IsTradable = false;
				RecruitsRoundShield.CanDropAsLoot = false;
				RecruitsRoundShield.Color = 0;
				RecruitsRoundShield.Bonus = 0; // default bonus				
				RecruitsRoundShield.Bonus1 = 1;
				RecruitsRoundShield.Bonus1Type = (int)2;
				RecruitsRoundShield.Bonus2 = 3;
				RecruitsRoundShield.Bonus2Type = (int)1;
				RecruitsRoundShield.Bonus3 = 13;
				RecruitsRoundShield.Bonus3Type = (int)1;
				RecruitsRoundShield.Bonus4 = 17;
				RecruitsRoundShield.Bonus4Type = (int)1;
				RecruitsRoundShield.Bonus5 = 19;
				RecruitsRoundShield.Bonus5Type = (int)1;
				RecruitsRoundShield.Bonus6 = 0;
				RecruitsRoundShield.Bonus6Type = (int)0;
				RecruitsRoundShield.Bonus7 = 0;
				RecruitsRoundShield.Bonus7Type = (int)0;
				RecruitsRoundShield.Bonus8 = 0;
				RecruitsRoundShield.Bonus8Type = (int)0;
				RecruitsRoundShield.Bonus9 = 0;
				RecruitsRoundShield.Bonus9Type = (int)0;
				RecruitsRoundShield.Bonus10 = 0;
				RecruitsRoundShield.Bonus10Type = (int)0;
				RecruitsRoundShield.ExtraBonus = 0;
				RecruitsRoundShield.ExtraBonusType = (int)0;
				RecruitsRoundShield.Effect = 0;
				RecruitsRoundShield.Emblem = 0;
				RecruitsRoundShield.Charges = 0;
				RecruitsRoundShield.MaxCharges = 0;
				RecruitsRoundShield.SpellID = 0;
				RecruitsRoundShield.ProcSpellID = 0;
				RecruitsRoundShield.Type_Damage = 1;
				RecruitsRoundShield.Realm = 3;
				RecruitsRoundShield.MaxCount = 1;
				RecruitsRoundShield.PackSize = 1;
				RecruitsRoundShield.Extension = 0;
				RecruitsRoundShield.Quality = 1;
				RecruitsRoundShield.Condition = 1;
				RecruitsRoundShield.MaxCondition = 1;
				RecruitsRoundShield.Durability = 100;
				RecruitsRoundShield.MaxDurability = 100;
				RecruitsRoundShield.PoisonCharges = 0;
				RecruitsRoundShield.PoisonMaxCharges = 0;
				RecruitsRoundShield.PoisonSpellID = 0;
				RecruitsRoundShield.ProcSpellID1 = 0;
				RecruitsRoundShield.SpellID1 = 0;
				RecruitsRoundShield.MaxCharges1 = 0;
				RecruitsRoundShield.Charges1 = 0;

				//You don't have to store the created item in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database

				GameServer.Database.AddObject(RecruitsRoundShield);
			}

			RecruitsSilveredBracelet = GameServer.Database.FindObjectByKey<ItemTemplate>("Recruits_Silvered_Bracelet");
			if (RecruitsSilveredBracelet == null)
			{
				RecruitsSilveredBracelet = new ItemTemplate();
				RecruitsSilveredBracelet.Name = "Recruit's Silvered Bracelet";
				if (log.IsWarnEnabled)
					log.Warn("Could not find " + RecruitsSilveredBracelet.Name + ", creating it ...");
				RecruitsSilveredBracelet.Level = 3;
				RecruitsSilveredBracelet.Weight = 1;
				RecruitsSilveredBracelet.Model = 124;
				RecruitsSilveredBracelet.Object_Type = 41;
				RecruitsSilveredBracelet.Item_Type = 33;
				RecruitsSilveredBracelet.Id_nb = "Recruits_Silvered_Bracelet";
				RecruitsSilveredBracelet.Hand = 0;
				RecruitsSilveredBracelet.Price = 2;
				RecruitsSilveredBracelet.IsPickable = true;
				RecruitsSilveredBracelet.IsDropable = true;
				RecruitsSilveredBracelet.IsTradable = false;
				RecruitsSilveredBracelet.CanDropAsLoot = false;
				RecruitsSilveredBracelet.Color = 0;
				RecruitsSilveredBracelet.Bonus = 0; // default bonus				
				RecruitsSilveredBracelet.Bonus1 = 10;
				RecruitsSilveredBracelet.Bonus1Type = (int)12;
				RecruitsSilveredBracelet.Bonus2 = 13;
				RecruitsSilveredBracelet.Bonus2Type = (int)1;
				RecruitsSilveredBracelet.Bonus3 = 0;
				RecruitsSilveredBracelet.Bonus3Type = (int)0;
				RecruitsSilveredBracelet.Bonus4 = 0;
				RecruitsSilveredBracelet.Bonus4Type = (int)0;
				RecruitsSilveredBracelet.Bonus5 = 0;
				RecruitsSilveredBracelet.Bonus5Type = (int)0;
				RecruitsSilveredBracelet.Bonus6 = 0;
				RecruitsSilveredBracelet.Bonus6Type = (int)0;
				RecruitsSilveredBracelet.Bonus7 = 0;
				RecruitsSilveredBracelet.Bonus7Type = (int)0;
				RecruitsSilveredBracelet.Bonus8 = 0;
				RecruitsSilveredBracelet.Bonus8Type = (int)0;
				RecruitsSilveredBracelet.Bonus9 = 0;
				RecruitsSilveredBracelet.Bonus9Type = (int)0;
				RecruitsSilveredBracelet.Bonus10 = 0;
				RecruitsSilveredBracelet.Bonus10Type = (int)0;
				RecruitsSilveredBracelet.ExtraBonus = 0;
				RecruitsSilveredBracelet.ExtraBonusType = (int)0;
				RecruitsSilveredBracelet.Effect = 0;
				RecruitsSilveredBracelet.Emblem = 0;
				RecruitsSilveredBracelet.Charges = 0;
				RecruitsSilveredBracelet.MaxCharges = 0;
				RecruitsSilveredBracelet.SpellID = 0;
				RecruitsSilveredBracelet.ProcSpellID = 0;
				RecruitsSilveredBracelet.Type_Damage = 0;
				RecruitsSilveredBracelet.Realm = 3;
				RecruitsSilveredBracelet.MaxCount = 1;
				RecruitsSilveredBracelet.PackSize = 1;
				RecruitsSilveredBracelet.Extension = 0;
				RecruitsSilveredBracelet.Quality = 1;
				RecruitsSilveredBracelet.Condition = 1;
				RecruitsSilveredBracelet.MaxCondition = 1;
				RecruitsSilveredBracelet.Durability = 100;
				RecruitsSilveredBracelet.MaxDurability = 100;
				RecruitsSilveredBracelet.PoisonCharges = 0;
				RecruitsSilveredBracelet.PoisonMaxCharges = 0;
				RecruitsSilveredBracelet.PoisonSpellID = 0;
				RecruitsSilveredBracelet.ProcSpellID1 = 0;
				RecruitsSilveredBracelet.SpellID1 = 0;
				RecruitsSilveredBracelet.MaxCharges1 = 0;
				RecruitsSilveredBracelet.Charges1 = 0;

				//You don't have to store the created item in the db if you don't want,
				//it will be recreated each time it is not found, just comment the following
				//line if you rather not modify your database

				GameServer.Database.AddObject(RecruitsSilveredBracelet);
			}

			#endregion


			// Custom Scriptloaded Code Begin
			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.AddHandler(Epona, GameLivingEvent.Interact, new DOLEventHandler(TalkToEpona));
			GameEventMgr.AddHandler(Epona, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToEpona));

			// Custom Scriptloaded Code End

			Epona.AddQuestToGive(typeof(ChangelingTimes));
			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			if (Epona == null)
				return;

			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.RemoveHandler(Epona, GameLivingEvent.Interact, new DOLEventHandler(TalkToEpona));
			GameEventMgr.RemoveHandler(Epona, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToEpona));

			/* Now we remove the possibility to give this quest to players */
			Epona.RemoveQuestToGive(typeof(ChangelingTimes));
		}

		protected static void TalkToEpona(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs)args).Source as GamePlayer;
			if (player == null)
				return;

			if (Epona.CanGiveQuest(typeof(ChangelingTimes), player) <= 0)
				return;

			//We also check if the player is already doing the quest
			ChangelingTimes quest = player.IsDoingQuest(typeof(ChangelingTimes)) as ChangelingTimes;

			Epona.TurnTo(player);
			//Did the player rightclick on Sir Quait?
			if (e == GameObjectEvent.Interact)
			{
				//We check if the player is already doing the quest
				if (quest != null)
				{
					//If the player is already doing the quest, we ask if he found the fur!
					if (quest.Step != 3)
						Epona.SayTo(player, "You have not found it yet?");
					else if (quest.Step == 3)
						quest.FinishQuest();
					return;
				}
				else
				{
					Epona.SayTo(player, "Hail, " + player.CharacterClass.Name + ". It's always good to see new adventurers willing to help out in the time of need. While it's understandable the garrison must be at the front lines, it's not acceptable to leave us defenseless.");
					Epona.SayTo(player, "With King Lug in Tir Na Nog you'd think he'd be more sympathetic to our needs. He's completely pre-occupied with this realm war though. Speaking of the realm war, a breach leading into Darkness Falls appeared not far from here. This breach brings the realm war even closer to Mag Mell than ever expected, as reports are filing in stating that Midgard and Albion have access to the dungeon somehow. Along with Demons' Breach, many unexplained occurrences were noticed. After the discovery, the fae creatures, especially the minor changelings, began acting oddly. We're concerned they may turn hostile. We need help in culling the population as they are quickly becoming a treat to the people of Mag Mell.");
					player.Out.SendQuestSubscribeCommand(Epona, QuestMgr.GetIDForQuestType(typeof(ChangelingTimes)), "Do you accept the quest? [Kill 2 minor changelings]");
					return;
				}
			}
		}

		protected static void SubscribeQuest(DOLEvent e, object sender, EventArgs args)
		{
			QuestEventArgs qargs = args as QuestEventArgs;
			if (qargs == null)
				return;

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(ChangelingTimes)))
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
			if (player.IsDoingQuest(typeof(ChangelingTimes)) != null)
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
			ChangelingTimes quest = player.IsDoingQuest(typeof(ChangelingTimes)) as ChangelingTimes;

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
			if (Epona.CanGiveQuest(typeof(ChangelingTimes), player) <= 0)
				return;

			if (player.IsDoingQuest(typeof(ChangelingTimes)) != null)
				return;

			if (response == 0x00)
			{
				SendReply(player, "Oh well, if you change your mind, please come back!");
			}
			else
			{
				//Check if we can add the quest
				if (!Epona.GiveQuest(typeof(ChangelingTimes), player, 1))
					return;
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
						return "Kill 2 minor changelings.";

					case 2:
						return "Kill 1 minor changelings.";

					case 3:
						return "Return to Epona once you have completed this task.";

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

			if (Step == 1 && e == GameLivingEvent.EnemyKilled)
			{
				EnemyKilledEventArgs gArgs = (EnemyKilledEventArgs)args;
				if (gArgs.Target.Name.IndexOf("minor changeling") >= 0)
				{
					SendSystemMessage("You've killed the " + gArgs.Target.Name);
					Step = 2;
					return;
				}
			}
			if (Step == 2 && e == GameLivingEvent.EnemyKilled)
			{
				EnemyKilledEventArgs gArgs = (EnemyKilledEventArgs)args;
				if (gArgs.Target.Name.IndexOf("minor changeling") >= 0)
				{
					SendSystemMessage("You've killed the " + gArgs.Target.Name);
					Step = 3;
					return;
				}
			}
		}

		public override void AbortQuest()
		{
			base.AbortQuest(); //Defined in Quest, changes the state, stores in DB etc ...
		}

		public override void FinishQuest()
		{
			base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			//Give reward to player here ...

			m_questPlayer.GainExperience(GameLiving.eXPSource.Quest, 22, true);
			long money = Money.GetMoney(0, 0, 0, 0, 10 + Util.Random(50));
			m_questPlayer.AddMoney(money, "You recieve {0} for your service.");
			GiveItem(m_questPlayer, RecruitsRoundShield, false);
			GiveItem(m_questPlayer, RecruitsSilveredBracelet, false);
			InventoryLogging.LogInventoryAction("(QUEST;" + Name + ")", m_questPlayer, eInventoryActionType.Quest, money);

		}
	}
}
