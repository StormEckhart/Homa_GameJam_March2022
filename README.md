# Homa_GameJam_March2022

How long did I work on this project ? :

I worked on this project for about 16h in total.


Why did I choose this theme and these mechanics ? :

I decided to go with theme of modelling clay (the ones for kids), as I thought it was a good way to make a colourful project, being able to create a runner stacker genre with it, and finally it's something people can relate to, kids or adults.
As I didn't have much time, I decided to create some basic yet effective mechanics : stacking clay into your ball, and when the ball is too small, to avoid hitting into walls, but when big enough, to splatter them.


Which parts of my code are reusable ? :

I always try to make my code as readable, reusable and optimized as possible.

Most of my projects have three main script types : Managers (unique singleton scripts, that manage a single part of the project, Game, UI, Level, Player etc...), Controllers (can be multiple of same type and handle most of the gameplay mechanics, Player, Enemies etc...), and References (that hold all needed references in a single script, instead of fetching components left and right, Level, Player, Enemies etc...).

Most of my Manager scripts are completely (InputManager, LevelManager, PlayerManager, CameraManager) or partially (the other manager scripts) reusable (copy pasting into new project).

My controllers are slightly more project focused, but have general methods that can be reused, or other more specific methods that with a bit of  tweaking can also.

My references scripts are completely project focused (but that's their purpose), but the core of some (LevelReferences or others) can easily be reused and added onto depending on the project at hand.
The same thing applies to my scriptableObjects.

Then all my "utility" scripts (Extenders, Tools etc...) are of course completely reusable.


What would I add if I were to develop this project into a fully fledged hypercasual game :

- I would firstly polish all existing visuals (signs and feedbacks (UI, particle systems etc...) included), and add others.
- I would change how the ball adds a new colour : I would colour only part of the ball with the new colour collected (with vertex colouring for example)
- I would then add a bonus mechanic that happens at the end of a level success (similar to Hair Challenge) where the player can throw the ball off the edge, into layers of platforms, the bigger the ball is, the more platforms it will break, and the further down it goes before scaling down to nothing, the more points the player would get.
- Now that I would have a "ideal" level, I could easily create new levels (not more that 40-50)
- I would lastly but not least add elements that would increase player rentention, such as :
. Player skins, or in this case ball skins
. Bonus rewards (similar to the game refs), such as collecting keys in game to open chests once the level finished, or gems to collect in game (that are needed in order to unlock skins)


