# Pandemic AI framework improvements
- [x] Create a performance - measuring project, so as to measure the upcoming improvements
- [x] Completely remove reflection - based comparison, to improve performance
- [x] Simplify the game - actions' class hierarchy, use interfaces instead, where possible
- [x] Create a method that generates the game's default definition, without the need for loading data from text
- [] Implement a visual debugging tool, that shows the game state and allows the user to select actions / macro - actions so as to check the game's progression

# intermediate game state
- [x] Implement an intermediate game state serialization class, to ease the transformation of the framework without losing ability to deserialize existing data
- [] Make sure that the normal game state can be properly converted to the minified game state
- [] Make sure that the minified game state can be properly converted back to the normal game state

# Refactoring - Debugging
- [] Make sure that the game's default definition is properly implemented, with no errors
- [] Embed the implementation of actions into the action - classes themselves, instead of inside the game class
- [] Add debugging methods (separated with #if DEBUG / #endif statements) in all of the actions' execution methods, to minimize the risk for bugs.