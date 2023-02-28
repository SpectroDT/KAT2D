# Kill All Them 2D Sample Project
## Player Side
### Goal
Survive the Bots invasion !
### Features
- Move player in a box (the box is smaller than the screen don't be surprise !)
- Shoot in front of you (white circle)
- Bots will spawn in wave
- Bots will move
- Bots will shoot (red circle)
- Bots don't have infinite ammo, they need to relaod sometimes (grey circle on the canon)
- Player have infinite ammo, don't hesitate to spam the fire button !
- Every (x) waves, a boss will spawn
- Sometimes (mostly on bosses) you can drop healing packs (yellow circle)
- Escape menu with a restart/exit button. You can see you're current wave and your highest wave here too (can you beat your highwave ?)
- The next wave will not wait you killed all bots to spawn, be careful of being surround by bots ! Only the boss wave will wait you done it.
- First 3 waves are the tutorial, after that, waves starting to be randomized !

## Dev Side
### Goal & Impression
First time I'm made a 2D SHMUP. It was a really fun experience to do !

I didn't follow any tutorial ! 

This sample was made in less than ~1.5 weeks.

### Others Infos
- AI can be dumb sometimes, need to improve the AI behaviour
- Endless wave until the player dies !  
- Wave can be empty, need to improve this !
- Wave timer can be long for nothing, need to calculate the timer in a better way
- No sound or music but can be add quite easly
- No background. I put a particle system just for having a "moving feeling" when playing

### What I would really want to improve
- AI Behaviour: Move and fire pattern
- Adding more Bots and Bosses for more fun !
- Wave Behaviour: wave timer/empty wave
- Game feeling: shaking the camera when getting hit, things like that
- Refactoring some stuff, adding some try/catch, adding some checks