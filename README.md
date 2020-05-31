# Pandemic AI framework
Α game playing AI framework for Pandemic (board game).

## Brief Game Description:
Pandemic is a cooperative board game, designed by Matt Leacock and first published by Z-Man Games in the United states in 2008.
The players' pawns are placed on the world map, in a situation where four deadly diseases have appeared and are spreading. During the game play, various random events, such as infections and outbreaks occur at new locations. The players can move on the map and attempt to prevent or correct these dangerous situations. In that process they may choose to "sacrifice" some of their cards in order to enhance their mobility. However, at the same time, the group must collaborate in gathering enough player cards to actually cure the four diseases, which is the game's ultimate goal. If the players do not manage to cure the diseases soon enough and the player cards run out, or if a high number of infections or outbreaks occurs, then the game is lost

## About the framework:
- The framework offers a quite optimized game state representation which can be used to develop planning - based AI agents that require a forward model. 
- Since the game state is both partially hidden and stochastic, the framework supports the implementation of Determinization techniques, via randomizing the hidden part of the state. 
- It also offers a selection of action - space. An agent can operate on the action - based representation, or a macros - based one, using the Macro Actions Synthesis System.
- A number of game state evaluation methods have been developed and are available for reuse.

## About the source - code:
The source code is currently being prepared for public sharing.
- [x] Prepare and upload the framework's source code
- [x] Provide examples of how to run computational experiments, using existing agents
- [x] Provide templates for the development of new agents
- [ ] Provide examples of how to query various types of information about the game state
- [ ] Provide examples of how to perform filtering operations on the set of available macro actions
- [ ] Prepare and upload the framework extension for running experiments with visuals


## Credits
The source code of this framework has been developed by [Konstantinos Sfikas](https://github.com/konsfik), in the context of his dissertation for the MSc in Digital Games, at the Institute of Digital Games (IDG) of the University of Malta, under the supervision of dr. [Antonios Liapis](https://github.com/sentientdesigns/).

The methodological approaches of this framework, including the problem's representation and the design of AI agents have been developed in collaboration between [Konstantinos Sfikas](https://github.com/konsfik) and dr. [Antonios Liapis](https://github.com/sentientdesigns/), as part of a broader, ongoing game playing AI research.
