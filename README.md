# ⚔️ The Duelists

A 2D pixel-art sword fighting game built in Unity, designed as an AI comparison demo. The core idea is to compare two AI decision-making approaches, a Finite State Machine and a Fuzzy Logic system, in a one-on-one dueling scenario.

---

## What Is This?

*The Duelists* showcases two AI approaches in a 2D side-scrolling fighting game. Both agents observe the same two inputs (their own health and the player's health) and use them to decide between three strategies: **Attack**, **Stand Ground**, or **Defend**.

The player and agents have equal health, damage, and attack cooldown. The player's only advantage is higher speed and the ability to jump, which keeps comparisons between the two AI systems fair.

---

## Tech Stack

| | |
|-|-|
| **Engine** | Unity 2022.3.62f3 |
| **Language** | C# |
| **AI Systems** | FSM (7 states) + Fuzzy Logic (4 membership function types) |
| **Art** | [Bandits - Pixel Art](https://assetstore.unity.com/packages/2d/characters/bandits-pixel-art-104130) + [Parallax Dusk Mountain Background](https://assetstore.unity.com/packages/2d/textures-materials/tiles/parallax-dusk-mountain-background-53403) |

---

## Game Modes

| Mode | Description |
|------|-------------|
| **Player vs FSM** | Fight a bandit controlled by a stack-based Finite State Machine |
| **Player vs Fuzzy** | Fight a bandit driven by a fuzzy inference engine |
| **FSM vs Fuzzy** | Watch the two AIs duel each other for side-by-side comparison |

---

## AI Systems

### Fuzzy Logic

Fuzzy Logic works with linguistically vague premises rather than crisp true/false values. Instead of asking "is the bandit healthy?", the system assigns degrees of membership across overlapping categories, allowing something like "somewhat low health" to influence behaviour in a gradual way.

The system evaluates `BanditHealth` and `PlayerHealth` (each ranging 0 to 100) as linguistic variables across three levels: `LOW`, `MEDIUM`, and `HIGH`. Four membership function shapes are used:

| Function | Shape |
|----------|-------|
| Triangular | Peak (Λ-shape) |
| Trapezoidal | Flat-top |
| Ascending Linear | 0 to 1 slope |
| Descending Linear | 1 to 0 slope |

These functions combine to produce an aggression value between 0 and 100. This is converted to a crisp number using centroid defuzzification (centre-of-gravity method), which takes the weighted centre of all active output functions.

**Fuzzy Rules (simplified):**

- **LowAggro** if BanditHealth is LOW and PlayerHealth is NOT LOW, or BanditHealth is MEDIUM and PlayerHealth is HIGH
- **MidAggro** if BanditHealth is MEDIUM and PlayerHealth is NOT LOW, or BanditHealth is LOW and PlayerHealth is LOW
- **HighAggro** if BanditHealth is HIGH and PlayerHealth is NOT HIGH, or BanditHealth is MEDIUM and PlayerHealth is LOW, etc.

The crisp aggression value maps to a behaviour:

| Aggression | Behaviour |
|------------|-----------|
| `>= 40` | **Offence** - get close and attack aggressively |
| `>= 30` | **Stand Ground** - hold position, attack if player enters range |
| `< 30` | **Defence** - retreat as far as possible, then hold |

> The real-time aggression value is displayed on screen in the Player vs Fuzzy scene.

### Finite State Machine (FSM)

FSMs represent agent behaviour as a graph of states with conditional transitions. This implementation uses a stack-based architecture: new states are pushed onto a `Stack<BaseState>`, and popping the stack returns the agent to its previous state. This preserves state history without needing to code every possible transition explicitly.

The FSM bandit has 7 states:

```
Idle -> ChasePlayer -> Attack -> StandGround -> RunAway -> Death
```

- Solid transitions push a new state onto the stack
- Dotted transitions pop the current state to return to the previous one
- Death is reachable from every state, since any behaviour can result in the agent taking a fatal hit

Transitions are driven by health differentials. The bandit chases when it has the health advantage and retreats when losing. Rather than coding a direct StandGround to ChasePlayer transition, the FSM pops back to Idle and pushes ChasePlayer, which reduces coupling between states.

### Comparing the Two

In a one-on-one scenario with only two inputs, both systems behave similarly and produce no dramatic difference in outcomes. The main practical tradeoffs are:

- **Fuzzy Logic** produces more gradual responses and scales better when more inputs are added, but requires more complex rules and recalculates membership functions on every update
- **FSM** is faster to implement, cheaper at runtime, and easier to reason about, but becomes harder to manage as state counts and transition conditions grow
- Both require careful planning upfront and fine-tuning after implementation

---

## Controls

| Action | Input |
|--------|-------|
| Move | Arrow keys / WASD |
| Jump | Space (when grounded) |
| Attack | Left mouse button |

---

### Scenes

| # | Scene | Description |
|---|-------|-------------|
| 0 | `MainScene.unity` | Main menu |
| 1 | `PlayerVSFsmScene.unity` | Player vs FSM bandit |
| 2 | `PlayerVSFuzzyScene.unity` | Player vs Fuzzy bandit (shows real-time aggression value) |
| 3 | `FsmVSFuzzyScene.unity` | AI vs AI comparison |

---

## Architecture Notes

- `Bandit.cs` supports both AI modes via boolean flags (`IsFsmActive`, `IsFuzzyLogicActive`)
- `IsSimulation` flag enables bandit-to-bandit combat for the AI vs AI scene
- FSM uses a `Stack<BaseState>` (push/pop) rather than a flat state graph
- Fuzzy sets are instantiated at game start; membership functions stay static while health inputs change at runtime
- Ground detection uses a separate `Sensor_Bandit` component (collision counter pattern)
- Damage is coroutine-based, synced to animation timing (~0.5s delay)
- Map boundary behaviour (RunAway/StandGround) emerges from environment collider positions

---
