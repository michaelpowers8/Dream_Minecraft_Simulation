# Dream Minecraft Simulation

This project simulates Dream's controversial Minecraft speedrun scenario, specifically the probability of obtaining Ender Pearls through piglin bartering and Blaze Rods from blazes in the Nether. The simulation runs millions of trials to analyze the likelihood of achieving the specific results Dream reported in his speedrun.

## Features

- **Monte Carlo Simulation**: Runs repeated trials of piglin bartering (262 trades) and blaze fights (305 fights)
- **Statistics Tracking**: Records:
  - Maximum Ender Pearls and Blaze Rods obtained in a single trial
  - Historical maximums for each resource
  - Averages across all trials
  - Best complete runs (where both resources were high)
- **Data Persistence**: 
  - Saves progress to JSON checkpoint files
  - Logs trial progress to XML files with timestamped entries
- **Automatic Log Rotation**: Keeps logs for 30 days before automatic deletion

## Technical Details

- **Piglin Bartering**: 20/423 chance per trade (≈4.73%)
- **Blaze Drops**: 1/2 chance per kill (50%)
- **Theoretical Averages**: 
  - Ender Pearls: ~12.3877 (262 trades × 20/423)
  - Blaze Rods: 152.5 (305 kills × 0.5)
- **Goal Values**: 
  - 42 Ender Pearls (Dream's reported result)
  - 211 Blaze Rods

## Project Structure

- `Program.cs`: Main simulation logic
- `Dream_Simulation_Checkpoint.json`: Persistent state storage
- `Dream_Simulation_Checkpoint_YYYY-MM-DD.xml`: Daily progress logs
- `Dream_Minecraft_Simulation.csproj`: Project configuration

## Usage

1. Build the project with .NET 8.0
2. Run the executable
3. The program will:
   - Load previous state from JSON if available
   - Continuously run trials
   - Save progress every:
     - New record for either resource
     - Every 10 million trials
   - Create daily XML logs
   - Auto-delete logs older than 30 days

## Results Interpretation

The simulation tracks how often the reported results (42 pearls + 211 rods) occur naturally, comparing against:
- Theoretical probabilities
- Running averages
- Historical maximums

Current best results after 129.93 billion trials:
- **Best Complete Run**: 35 pearls + 185 rods
- **Most Pearls Ever**: 42 (with 160 rods)
- **Most Rods Ever**: 213 (with 13 pearls)

## Requirements

- .NET 8.0 SDK
- Newtonsoft.Json package