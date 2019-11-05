# rts-game

This was my first attempt to make a RTS game. It served as my bachelor's thesis in 2018.

Content:
- 3 military units - axeman (light infantry), archer, knight (heavy infantry)
- resource gatherer with limited offensive capabilities

Features:
- coordinated movement with obstacle avoidance for groups of units. There are 3 formations: line, rectangle and column
- multiple unit stances - passive, defensive, aggressive and follow target
- easily customizable AI state machine based on scriptable objects
- an extendable influence map - used as a heat map by AI. A high heat area is an area with a lot of enemy units.

[Architecture diagrams](./architecture/architecture.md)

Can be improved:
- the AI framework can be converted / extended to a behaviour tree while keeping the scriptable object architecture
- game UX - as this was an academic project and the dev time rather limited, the UI and controls were not a priority
- buildings 😁
