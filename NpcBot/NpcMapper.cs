/*
 * NpcMapper
 * 1.0.0
 * Troispoils
 * 09/12/2012
 * 
 * Un NPC Mappeur qui ajoute different mob a different endroits ^^
 *
*/

using System;
using System.Reflection;
using System.Collections;

using DOL;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOL.Events;
using DOL.Database;
using DOL.GS.Styles;
using DOL.GS.Utils;
using DOL.GS.Quests;
using DOL.GS.Housing;
using DOL.GS.Movement;

using DOL.AI;
using DOL.AI.Brain;

using log4net;

namespace DOL.GS.Trois
{
	public class NpcMapper : GameNPC
	{
		public readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public bool debugInf = true;

		//constructor
		public NpcMapper()
            : base()
        {
			Flags = GameNPC.eFlags.PEACE; //force the peace state
		}

		//definition of the mapper NPC
		public override bool AddToWorld()
		{
			Model = 140;
			Size = 55;
			Name = "Péon";
			GuildName = "Builder";
			Realm = eRealm.None;
			Level = 50;

			if (debugInf)
				log.Warn("[DEBUG] AJOUTE LE MAPPERNPC DANS LE MONDE");

			//mobs_list();

			return base.AddToWorld();
		}
	}
}

namespace DOL.AI.Brain
{
	public class AIMapper : StandardMobBrain
	{
		//private StandardMobBrain brain = null;
		private bool Walking;

		//constructor
		public AIMapper()
			: base()
		{
			ThinkInterval = 20000;
			Walking = false;
		}

		public override void Think()
		{
			DOL.GS.Trois.NpcMapper NPC = Body as DOL.GS.Trois.NpcMapper;

			int[] Pos = new int[2];

			

			base.Think();
		}
	}
}
