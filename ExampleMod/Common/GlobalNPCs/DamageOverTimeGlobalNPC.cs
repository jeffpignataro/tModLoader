﻿using ExampleMod.Content.Buffs;
using ExampleMod.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Common.GlobalNPCs
{
	internal class DamageOverTimeGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool exampleJavelinDebuff;

		public override void ResetEffects(NPC npc) {
			exampleJavelinDebuff = false;
		}

		public override void SetDefaults(NPC npc) {
			// TODO: This doesn't work currently. tModLoader needs to make a fix to allow changing buffImmune

			// We want our ExampleJavelin buff to follow the same immunities as BoneJavelin
			npc.buffImmune[ModContent.BuffType<ExampleJavelinDebuff>()] = npc.buffImmune[BuffID.BoneJavelin];
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (exampleJavelinDebuff) {
				if (npc.lifeRegen > 0) {
					npc.lifeRegen = 0;
				}
				// Count how many ExampleJavelinProjectile are attached to this npc.
				int exampleJavelinCount = 0;
				for (int i = 0; i < 1000; i++) {
					Projectile p = Main.projectile[i];
					if (p.active && p.type == ModContent.ProjectileType<ExampleJavelinProjectile>() && p.ai[0] == 1f && p.ai[1] == npc.whoAmI) {
						exampleJavelinCount++;
					}
				}
				// Remember, lifeRegen affects the actual life loss, damage is just the text.
				// The logic shown here matches how vanilla debuffs stack in terms of damage numbers shown and actual life loss.
				npc.lifeRegen -= exampleJavelinCount * 2 * 3;
				if (damage < exampleJavelinCount * 3) {
					damage = exampleJavelinCount * 3;
				}
			}
		}

	}
}
