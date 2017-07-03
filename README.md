## Build and running the app

1. Install npm dependencies: `yarn install`
2. Install dotnet dependencies: `dotnet restore`
3. Start Fable server and Webpack dev server: `dotnet fable npm-run start`
4. In your browser, open: [http://localhost:8080/](http://localhost:8080/)

Any modification you do to the F# code will be reflected in the web page after saving.

## Plot Notes
- Pompeia is chasing the feared bandit Lord Greltza.
- Greltza has stolen the Heartseed that keeps the town's defender alive, and she is slowly dying.
- Greltza will flee to the Ereshkigal Mountains, then through the Desert of Ten Thousand Dunes, before being caught on
  the shores of the Great Inland Sea.
- While going through the Desert of Ten Thousand Dunes, Pompeia will run into the Desert Knight, a mummified Rearkip who
  has been stalking the desert for two hundred eighty-two years fighting bandits (as he did in life)

## World Notes
- Dominant race: the Rearkip, intelligent humanoid bat people.

## Design Notes
- Add money to the game, allow bribery
- When should conditions go away?  Should there even be conditions?
    - If we have conditions, we need to think about what to do when the player is brought unconscious.
    - Maybe have them start the act over?  If the acts are fairly short, this could work.
        - Would make saving slightly more complicated, but that should be okay.