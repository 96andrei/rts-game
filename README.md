# rts-game

This was my first attempt to make a RTS game. It served as my bachelor's thesis.
Content:
- 3 military units - axeman (light infantry), archer, knight (heavy infantry)
- resource gatherer with limited offensive capabilities

Features:
- coordinated movement with obstacle avoidance for groups of units. There are 3 formations: line, rectangle and column
- 3 unit stances: passive (does not react to attacks), defensive (will follow in a short range), offensive (will always follow)
- easily customizable AI state machine based on scriptable objects
- an extendable influence map - used as a heat map by AI. A high heat area is an area with a lot of enemy units.