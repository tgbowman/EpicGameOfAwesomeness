using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EpicGameAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EpicGameAPI.Data;
using System.Collections.Generic;

namespace BagoLootAPI.Data
{
    public static class DbInitializer
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var userstore = new UserStore<User>(context);

                if (!context.Roles.Any(r => r.Name == "Administrator"))
                {
                    var role = new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" };
                    await roleStore.CreateAsync(role);
                }

                if (!context.User.Any(u => u.Email == "admin@admin.com"))
                {
                    //  This method will be called after migrating to the latest version.
                    User user = new User
                    {
                        UserName = "admin@admin.com",
                        NormalizedUserName = "ADMIN@ADMIN.COM",
                        Email = "admin@admin.com",
                        NormalizedEmail = "ADMIN@ADMIN.COM",
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };
                    var passwordHash = new PasswordHasher<User>();
                    user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
                    await userstore.CreateAsync(user);
                    await userstore.AddToRoleAsync(user, "Administrator");
                }

                // Look for any products.
                if (context.UnitClass.Any())
                {
                    return;   // DB has been seeded
                }

                var unitClass = new UnitClass[]
                {

                    new UnitClass {
                        Name = "Basic",

                        AbilityOneName = "Punch",
                        AbilityOneDescription = "Lash out with your fists and smack the shit out of the enemy!",
                        AbilityOneDamage = 15,

                        AbilityTwoName = "Slap of Death",
                        AbilityTwoDescription = "Slap the life right out of the enemy!",
                        AbilityTwoDamage= 50
                    },

                };

                foreach (UnitClass u in unitClass)
                {
                    context.UnitClass.Add(u);
                }
                context.SaveChanges();

                var characters = new Character[]
                {
                    new Character {
                        Name = "Mordran",
                        UnitClassId = unitClass.Single(u => u.Name == "Basic").Id,
                        HP = 100,
                        User = context.User.Single(user => user.Email == "admin@admin.com")

                    },

                };

                foreach (Character c in characters)
                {
                    context.Character.Add(c);
                }
                context.SaveChanges();


                var enemy = new Enemy[]
                {
                    new Enemy {
                        Name = "Zombie",
                        UnitClassId = unitClass.Single(u => u.Name == "Basic").Id,
                        HP = 100,
                        Boss = false
                    },
                    new Enemy {
                        Name = "Lich",
                        UnitClassId = unitClass.Single(u => u.Name == "Basic").Id,
                        HP = 75,
                        Boss = false
                    },
                    new Enemy {
                        Name = "Balrog",
                        UnitClassId = unitClass.Single(u => u.Name == "Basic").Id,
                        HP = 200,
                        Boss = true
                    }
                };

                foreach (Enemy e in enemy)
                {
                    context.Enemy.Add(e);
                }
                context.SaveChanges();

                var adventure = new Adventure[]
                {
                    new Adventure {
                        Title = "The Epic One"
                    },
                };

                foreach (Adventure a in adventure)
                {
                    context.Adventure.Add(a);
                }
                context.SaveChanges();

                var roadBlock1 = new RoadBlock();
                roadBlock1.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                roadBlock1.StartingPoint = true;
                roadBlock1.Description = "You are a loyal member of the Kings Army.  One evening, while on patrol, a woman rushes up to you and says 'It took my necklace!!  That THING took the necklace that's been in my family for generations!' You look past her to see a large creature skulking into the woods nearby.";
                roadBlock1.PreviousOptionId = null;

                context.RoadBlock.Add(roadBlock1);
                context.SaveChanges();

                var options1 = new PathOption[]
                {
                    new PathOption
                    {
                        Description = "Rush into the woods after the creature!",
                        LeadsToCombat = false
                    },
                    new PathOption
                    {
                        Description = "Keep on walking, this isn't any of your concern.",
                            LeadsToCombat = true
                    }
                };

                foreach(PathOption p in options1)
                {
                    context.PathOption.Add(p);

                    StoryPath path = new StoryPath();

                    path.RoadBlockId = roadBlock1.Id;
                    path.PathOptionId = p.Id;

                    context.StoryPath.Add(path);
                    
                }
                context.SaveChanges();

                var roadBlock2_1 = new RoadBlock();
                roadBlock2_1.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                roadBlock2_1.Description = "As you enter the woods the world goes black.  The forrest is so dark you cannot see.  You hear nothing but the wind rushing through the branches.";
                roadBlock2_1.PreviousOptionId = options1.Single(p => p.Description == "Rush into the woods after the creature!").Id;
                roadBlock2_1.StartingPoint = false;

                context.RoadBlock.Add(roadBlock2_1);
                context.SaveChanges();

                var options2_1 = new PathOption[]
                {
                    new PathOption
                    {
                        Description = "Feel around in the darkness for something to make a torch.",
                        LeadsToCombat = false
                    },

                    new PathOption
                    {
                        Description = "Rush ahead blindly, the creature is getting away!",
                        LeadsToCombat = true
                    }
                };

                foreach (PathOption p in options2_1)
                {
                    context.PathOption.Add(p);

                    StoryPath path = new StoryPath();

                    path.RoadBlockId = roadBlock2_1.Id;
                    path.PathOptionId = p.Id;

                    context.StoryPath.Add(path);

                }
                context.SaveChanges();

                RoadBlock Woods_1 = new RoadBlock();
                Woods_1.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                Woods_1.Description = "You feel desparately on the ground for anything that will burn.  You find a dry branch, tear the sleae of your tunic off and wrap the top of the branch with it.  You use flint to spark the cloth alight.  You press on into the forrest.  You come to a clearing blanketed in low hanging fog.  The air feels cold, and across the clearing you see the necklace lying on the stump of a tree.  You rush forward but stop short as a figure glides around from the forrest edge towards the necklace.  The figure is floating half a foot off the ground and is cloaked in a black decrepit looking robe embroidered with glittering runes.  Its skin is pale and you can see spots where the flesh is decaying.  You recognize the creature as a Lich, an undead sorceror.  The Lich doesn't appear to have noticed your presence.  It turns towards the necklace and green magic shoots from its fingers!  The necklace rises from the stump.  The lich is transfering its essence into the neckalce to create a phylactory!  If it succeeds it will become immortal and torment the nearby village!";
                Woods_1.PreviousOptionId = options2_1.Single(o => o.Description == "Feel around in the darkness for something to make a torch.").Id;
                Woods_1.StartingPoint = false;

                context.RoadBlock.Add(Woods_1);
                context.SaveChanges();

                RoadBlock Woods_2 = new RoadBlock();
                Woods_2.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                Woods_2.Description = "You run with your arms splayed in front of you feeling for any trees that might be in your way.  Brush snags at your trousers.  You feel wiry twigs claw at your face.  Suddenly the ground beneath your feet disapears.  You fall off the edge of a cliff and plummett to your death.";
                Woods_2.GameOver = true;
                Woods_2.PreviousOptionId = options2_1.Single(o => o.Description == "Rush ahead blindly, the creature is getting away!").Id;
                Woods_2.StartingPoint = false;

                context.RoadBlock.Add(Woods_2);
                context.SaveChanges();

                






                RoadBlock roadBlock2_2 = new RoadBlock();
                roadBlock2_2.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                roadBlock2_2.Description = "As you turn to leave the woman becomes angry.  You turn away to head back towards the barracks.  You hear cloth tearing, bones breaking, and a bone chilling howl from behind you.";
                roadBlock2_2.PreviousOptionId = options1.Single(o => o.Description == "Keep on walking, this isn't any of your concern.").Id;
                roadBlock2_2.StartingPoint = false;

                context.RoadBlock.Add(roadBlock2_2);
                context.SaveChanges();

                var options2_2 = new PathOption[]
                {
                    new PathOption()
                    {
                        Description = "Stop and look back.",
                        LeadsToCombat = false
                    },

                    new PathOption()
                    {
                        Description = "RUN FAST!",
                        LeadsToCombat = false
                    }
                };

                foreach(PathOption p in options2_2)
                {
                    context.PathOption.Add(p);

                    StoryPath path = new StoryPath();
                    path.RoadBlockId = roadBlock2_2.Id;
                    path.PathOptionId = p.Id;

                    context.StoryPath.Add(path);
                }
                context.SaveChanges();

                RoadBlock roadBlock3_1 = new RoadBlock();
                roadBlock3_1.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                roadBlock3_1.Description = "As you turn you see an enormous beast where the woman once stood.  The beast stands on two legs, and has facial features not unlike the wolves you protect the villagers from.  Clearly the woman was hiding her true nature from you.  The werewolf bares it's claws and belows a challanging roar towards you!";
                roadBlock3_1.PreviousOptionId = options2_2.Single(p => p.Description == "Stop and look back.").Id;
                roadBlock3_1.StartingPoint = false;

                context.RoadBlock.Add(roadBlock3_1);
                context.SaveChanges();

                RoadBlock roadBlock3_2 = new RoadBlock();
                roadBlock3_2.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                roadBlock3_2.Description = "You run faster than you've ever run before.  From behind you comes a ground shaking roar and you feel the ground shaking beneath you.  You steal a glance behind you and see an enormous creature barelling towards you!  Clearly the woman wasn't telling you the entire truth.  The woman, now in the form of a huge werewolf, grabs you by the leg and you topple to the ground.  You roll over on your back to see the behemoth tower above you!  It raises it's claws and roars!";
                roadBlock3_2.StartingPoint = false;
                roadBlock3_2.PreviousOptionId = options2_2.Single(p => p.Description == "RUN FAST!").Id;

                context.RoadBlock.Add(roadBlock3_2);
                context.SaveChanges();

                PathOption option3_1 = new PathOption();
                option3_1.Description = "Draw your weapon and prepare for battle!";
                option3_1.LeadsToCombat = true;

                context.PathOption.Add(option3_1);
                context.SaveChanges();

                StoryPath path3_1 = new StoryPath();
                path3_1.PathOptionId = option3_1.Id;
                path3_1.RoadBlockId = roadBlock3_1.Id;

                context.StoryPath.Add(path3_1);
                context.SaveChanges();

                PathOption option3_2 = new PathOption();
                option3_2.Description = "You roll out of reach of the beasts immense claws.  Get to your feet and draw your weapon.  This Isn't going to be easy.";
                option3_2.LeadsToCombat = true;

                context.PathOption.Add(option3_2);
                context.SaveChanges();

                StoryPath path3_2 = new StoryPath();
                path3_2.PathOptionId = option3_2.Id;
                path3_2.RoadBlockId = roadBlock3_2.Id;

                RoadBlock roadBlock4_Final = new RoadBlock();
                roadBlock4_Final.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                roadBlock4_Final.Description = "The werewolf let's out one final gutteral howl as it drops lifelessly to the ground.  You dust yourself off, sheath your sword and head to the nearest tavern.  You deserve a drink!";
                roadBlock4_Final.PreviousOptionId = option3_1.Id;
                roadBlock4_Final.StartingPoint = false;

                context.RoadBlock.Add(roadBlock4_Final);
                context.SaveChanges();

                RoadBlock roadBlock4_2Final = new RoadBlock();
                roadBlock4_2Final.AdventureId = adventure.Single(a => a.Title == "The Epic One").Id;
                roadBlock4_2Final.Description = "The werewolf let's out one final gutteral howl as it drops lifelessly to the ground.  You dust yourself off, sheath your sword and head to the nearest tavern.  You deserve a drink!";
                roadBlock4_2Final.PreviousOptionId = option3_1.Id;
                roadBlock4_2Final.StartingPoint = false;

                context.RoadBlock.Add(roadBlock4_2Final);
                context.SaveChanges();













            }
        }
    }
}