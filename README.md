# Chaotic Skills
Adds a bunch of alternate skills!

## Captain
Beacon: Design (Misc) - Emits waves that cripple and knockback ALL characters.

Beacon: Void (Misc) - Vo??id Fo??g slowly damages enemies within the radius.

Offensive Microbots (Passive) - Gain 3 combatant microbots that fire slowing lasers at nearby enemies for 160% damage, and one alongside you for 360% damage

Spark Cannon (Primary) - Channel an electric beam for 340% damage. Deals critical hits after 3 seconds.

Crash Remote (Utility) - Call down reinforcements from the UES Safe Travels, has a long cooldown.

## Commando
SFG-900 (Primary) - Stunning. Launch a slow-moving energy ball for 700% damage.

Frost Grenade (Special) - Freezing. Toss a grenade that explodes in an icy blast. Does no damage, but freezes targets.

Barrage (Secondary) - Stunning. Fire 3 targeted rockets for 400% damage.

## Engineer
TR-80 Sniper Turret (Special) - Place a turret that inherits all of your items. Fires a piercing bolt for 900% that slows. Can place up to 1.

PM-30 Support Turret (Special) - Place a turret that inherits all of your items. Heals it's owner and provides them with 8 seconds of invulnerability and guaranteed critical hits after enough healing. Can place up to 1

BFT-9000 (Special) - Place a massive turret that inherits all of your items. Fires a giant fucking laser with a massive blast radius for 13,567.5% 3.0 on a long cooldown. Can place up to 1.

Overdrive Boost (Utility) - Agile. Stunning. Launch into the air and boost forward after a short delay. Deal 100%-2500% damage depending on fall speed. Immunity to fall damage while boosting.

Thermal Blast (Primary) - Agile. Ignite. Fire dual flamethrowers for 600% damage.

Constructor Pylon (Special) - Place a stationary device that produces clones of it's targets periodically. Clones inherit your items.

## Mercenary
Phantom Strike (Primary) - Unleash a flurry of 3 phantom mercenaries that each slash for 200% damage.

Fluoresence (Secondary) - Agile. Tether enemies, dragging them towards you and repeatedly strike them for 15% damage. 50% slow on self when active.

Momentum Capacitors (Passive) - Killing airborne enemies grants a stacking 10% boost to damage. All stacks reset upon touching the ground.

## Artificer
Overcharged Jetpack (Passive) - Hold jump midair to fly. Flying drains fuel. Fuel passively regenerates over time. Running out of fuel will temporarily disable the jetpack.

Fracture (Primary) - Charge up a lunar bolt for 300%-1100% damage.

Flamerang (Primary) - Ignite. Swing a flaming sawblade for 400% damage per second.

M1A2 SEPV3 Main Battle Tank (Utility) - Deploy a rideable M1A2 SEPV3 Main Battle Tank. Primary fire launches a rocket for 2500% damage.

## Bandit
Sadism (Passive) - Your attacks strike an additional time for each debuff on the target, dealing 50% TOTAL damage.

Feign (Utility) - Negate the next instance of damage and warp behind the attacker. Grants your next attack quadruple damage.

## MUL-T
Auto-Nailblast (Primary & Misc) - Fire a stream of shotgun blasts for 6x90% damage. 50% slow on self when active.

Heat Flush (Primary & Misc) - Fire a volley of molten slag, dealing 180% ignite damage and setting terrain ablaze.

## Huntress
Destroyer (Special) - Charge up a piercing blade for 900%-3700% damage. Emits a stunning shockwave when fully charged. 50% slow on self when charging.

## Acrid
Pathosis (Passive) - Attacks that apply poison instead apply Pathosis. Targets afflicted with Pathosis share damage taken with the nearby afflicted. 7.5s duration

## Railgunner
HR-30 Redirector (Primary) - Toss a device that redirects shots into nearby enemies when struck, amplifying their damage. Hold up to 4.

# Changelog
## 1.8.3
- updated BepInIncompatibility
## 1.8.2
- Added config for uncapping allies with lysate cell
## 1.8.1
- made artificer alt jetpack not make a brand new state machine
## 1.8.0
- HOLY SHIT IS THAT AN ULTRAKILL REFERENCE (added HR-30 Redirector as an alternate primary for Railgunner)
- fixed certain skills being configurable when they shouldn't (turret primaries)
- made flamerang agile
## 1.7.6
- fixed captain beacons and mul-t primaries not being configurable
## 1.7.5
- added Flamerang as an alternate primary for Artificer
## 1.7.0
- added Feign as an alternate utility for Bandit
- aded Phantom Strike as an alternate primary for Mercenary
- added Heat Flush as an alternate primary/misc for MUL-T
## 1.6.2
- changed TR-80 Sniper Turret to fire a single, more powerful shot at a much larger range
- fixed Barrage getting canceled early by other skills and itself
- changed the icon
- added R2API_Networking to the dependencies (it was required since 1.6.0 but i forgor to include it)
## 1.6.1
- fixed loadout tab conflict between passiveaggression and this
## 1.6.0
- networked nearly everything (tank isnt networked)
- fixed pylon having no cap
- fixed sprinting issues with offensive microbots
- fixed pylon not properly adding the material
- fixed pylon cloning bosses
- switched pylon fallback spawns to chimera when on commencement
- moved spark cannon vfx to originate from the eye cross so it doesnt wobble everywhere
## 1.5.0
- removed gaze of the void
- added Spark Cannon as an alternate primary for captain
- added Barrage as an alternate secondary for commando
- upped fracture damage to 1100% max and removed homing
- added Constructor Pylon as an alternate special for the engineer
- added enable/disable configs for skills
## 1.4.0
- buffed fracture to be 300%-960%
- improved homing on fracture
- added Momentum Capacitors as an alternate passive for merc
- added Sadism as an alternate passive for bandit
- added Frost Grenade as an alternate special for commando
- added Destroyer as an alternate special for huntress
- added Auto-Nailblast as an alternate primary & misc for MUL-T
- added Pathosis as an alternate passive for acrid
- added M1A2 SEPV3 Main Battle Tank as an alternate utility for artificer
- fixed the engineer turrets killing every single ally on spawn
- new bft visuals (made by violet)
- fixed overdrive boost not giving impact damage and fall immunity
## 1.3.0
- added config for BFT-9000 damage
- made the PM-30 support turret not break when frozen
- the SFG-900 can no longer be sprint canceled to negate the firing delay
- engineer turrets no longer get canceled if you try and sprint while placing one
- made BFT-9000 more accurate
- added Fracture as an alternate primary for artificer
- added Overcharged Jetpack as an alternate passive for artificer
- added Crash Remote as an alternate utility for captain
- improved networking
## 1.2.0
- holy shit is that a doom reference?????
## 1.1.1
- sniper turrets now actually roll for critical hits
## 1.1.0
- fixed thermal blast doing 100x the damage it was intended to
- offensive microbots should break other mods less often
- made alt engi turrets work with lysate cell
- reworked void gaze
- increase blast radius on sfg-900
## 1.0.1
- readme fix lmao
