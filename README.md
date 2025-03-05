# Roulette Game Documentation

## 1. Overview

This repository contains a fully structured Roulette game implemented in Unity. It includes betting mechanics, chip management, and a Bet Data Editor to automate bet configurations.

## Gameplay Demo
[![Roulette Gameplay](https://img.youtube.com/vi/NXZRyQRY2eU/0.jpg)](https://www.youtube.com/watch?v=NXZRyQRY2eU)

---
## 2. Features

- Betting System: Supports Inside and Outside Bets.
- Chip Management: UI-integrated betting with different denominations.
- Automated Bet Data Generation: Generates all possible bets as ScriptableObjects.
- Optimized Gameplay: Efficient bet evaluations and structured data storage.
- Pre-determined Winning Number Selection: Players can select the next winning number by long pressing on the roulette UI.
- Object-Oriented Programming (OOP) Design: Implements best practices including Encapsulation, Inheritance, Polymorphism, and SOLID Principles.
- Bet Undo & Clear Buttons: Players can undo the last placed bet or clear all placed bets. These buttons are located next to the Spin button in the bottom-right corner of the UI.
- Stack-based Bet Removal: If no chip is selected, tapping the stacked chips removes the most recently placed bet.

---
## 3. Betting System

### 3.1 Bet Types

#### Inside Bets:

- Straight → Single number (e.g., 7) - Payout: 35:1
- Split → Adjacent numbers (e.g., 1-2, 1-4) - Payout: 17:1
- Street → Three consecutive numbers in a row (e.g., 1-2-3) - Payout: 11:1
- Corner → Four-number intersections (e.g., 2-3-5-6) - Payout: 8:1
- Six Line → Two adjacent Streets (e.g., 1-2-3-4-5-6) - Payout: 5:1

#### Outside Bets:

- Red/Black, Even/Odd, High/Low → Covers 18 numbers - Payout: 1:1
- Dozens (1-12, 13-24, 25-36) → Covers 12 numbers - Payout: 2:1
- Columns → Covers 12 numbers (Vertical) - Payout: 2:1

---
## 4. Pre-determined Winning Number Selection

In the roulette table UI, players can long-press on a number to pre-select it as the next winning number.

- The selected number will be highlighted.
- Long-pressing an already selected number will deselect it.
- If no number is selected, the outcome will be randomized.
- Only one number can be selected at a time.
- The game will use the selected number as the result unless it is deselected before the spin.

---
## 5. Chip System

- Five Chip Denominations: 1, 5, 10, 50, 100
- Chip UI Interaction: Players can select a chip and place bets.
- Bet Validation: Ensures proper bet placement and calculation.
- Stacked Chip Removal: If no chip is selected, tapping the stacked chips removes the last placed bet.

---
## 6. OOP Principles in Roulette Betting System 🎯

### 6.1 Encapsulation

- Private fields with getters: `Chip.Value` and `BetCondition.TargetNumbers` are private and only modifiable via controlled methods.
- Controlled access via methods: `Evaluate(int winningNumber)` ensures bet evaluations are handled internally.

### 6.2 Inheritance

- `Bet` is the base class for all bets.
- `RouletteBet` extends `Bet` and implements game-specific logic.

```csharp
public abstract class Bet
{
    public BetType BetType { get; protected set; }
    public int Amount { get; protected set; }

    protected Bet(BetType betType, int amount)
    {
        BetType = betType;
        Amount = amount;
    }

    public abstract float ResolveBet(int winningNumber);
}
```

### 6.3 Polymorphism

- `IBetCondition` is an interface implemented by `BetCondition`, allowing multiple evaluation strategies.
- `Evaluate(int winningNumber)` behaves differently depending on the bet type.

```csharp
public interface IBetCondition
{
    bool Evaluate(int winningNumber, int[] numbers);
}
```

### 6.4 Abstraction

- `BetCondition` hides the logic of how bets are evaluated.
- `RouletteBet` abstracts the mechanics of bet resolution.

---
## 7. SOLID Principles

✅ Single Responsibility Principle (SRP)
- `BetCondition` only evaluates bet outcomes.
- `BetData` only stores bet-related information.

✅ Open/Closed Principle (OCP)
- `IBetCondition` allows for new bet evaluation strategies without modifying existing code.

✅ Liskov Substitution Principle (LSP)
- `RouletteBet` can replace `Bet` without breaking functionality.

✅ Interface Segregation Principle (ISP)
- `IBetCondition` ensures that bet evaluation logic remains separate from other game logic.

✅ Dependency Inversion Principle (DIP)
- `BetData` depends on `IBetCondition`, not concrete implementations, making it easily extendable.

---
## 8. Bet Data Editor

### 8.1 Overview

The BetDataEditor automates the creation of bet data and conditions in Unity.

### 8.2 Features

- Automatic Bet Data Creation: Generates ScriptableObjects for each bet type.
- Categorized Storage: Saves bet data under structured folders based on bet types.
- Condition Handling: Ensures bet conditions are created or reused if they already exist.

### 8.3 How It Works

#### 1. Opening the Editor

Navigate to Unity Editor → Tools → Generate American Roulette Bets.

#### 2. Generating Bet Data

Click "Generate Bet Data", and the tool will:

- Create a BetData folder inside `Assets/BetData`.
- Organize bet data into subfolders based on bet types.
- Generate or reuse BetCondition ScriptableObjects.

---
## 9. How to Play

- Unity Version: Developed in Unity 2022.3.20, compatible with later versions.
- Render Pipeline: Uses URP (Universal Render Pipeline).
- Gameplay Flow:
  1. Select a chip denomination.
  2. Tap on the roulette table to place a bet.
  3. Long-press a number to pre-select it as the next result (optional).
  4. Press Spin to start the roulette.
  5. The latest result appears on UI, previous results can be scrolled in the Results Panel.
  6. Winning bets are calculated and paid out accordingly.

---
## 10. Future Improvements

- **Persistent Data System:** Currently, the game starts with a fixed amount of 1000 chips. This value can be adjusted in the Unity Inspector under `ChipManager -> Starting Chip Count`.
- **Auto-Save Feature:** The game does not yet persist data across sessions, but an auto-save feature can be implemented to store:
  - Active bets
  - Total chips
  - Last spin outcome
  - Other game state variables

---
## 11. License

MIT License - Free to use and modify.

End of Document
