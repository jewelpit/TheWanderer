module Wanderer.Pages

open Fable.Import.React

open Wanderer.Skills

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

type FailureEffect =
    | AlternatePage of string
    | AttributeDamage

type Condition =
    | Automatic
    | SkillCheckRequired of Attribute * Skill * int * FailureEffect
    | Bribe of int
    | Flags of string list * Condition

type Continuation = {
    Description : string
    NextPageName : string
    Condition : Condition
    SetFlags : string list
    GrantsMoney : int
}

type ContinuationBuilder() =
    member __.Build(description, nextPageName, ?condition, ?setFlags, ?grantsMoney) =
        {
            Description = description
            NextPageName = nextPageName
            Condition = defaultArg condition Automatic
            SetFlags = defaultArg setFlags []
            GrantsMoney = defaultArg grantsMoney 0
        }

type Page = {
    Name : string
    Text : string list
    Continuations : Continuation list
    Resets : bool
}

type PageBuilder() =
    member __.Build(name, text, continuations, ?resets) =
        {
            Name = name
            Text = text
            Continuations = continuations
            Resets = defaultArg resets false
        }

let private cb = ContinuationBuilder()
let private pb = PageBuilder()

let pages =
    [
        pb.Build(
            "start",
            [
                """I first made my way into Tetznatalk as dusk was falling, after a long day of travel.  I was
                weary, and looking forward to my first night indoors in nearly a month.  I rode up to the inn and
                stabled my [[monitor beetle|Monitor Beetle]], relieved that the inn seemed to have facilities to care
                for it."""
                """When I walked into the bar, I saw a shaky, bespectacled Etzen behind the counter, nervously
                cleaning a glass.  He was your typical rural farmer, dressed in the round black hat and brown kilt, and
                looked lost in thought.  At the sound of my arrival he looked up, and before he could stop blurted out,
                "Please!  You must help us!" """
            ],
            [cb.Build("\"Excuse me?\"", "tez1"); cb.Build("Enter test harness", "middle")])
        pb.Build(
            "tez1",
            [
                """He apologized, and told me that he was just desperate.  Lady Greltza, a local [[bandit|Bandit]] lord,
                had managed to trick the [[Town Guardian]] and steal her heartseed, without which she was slowly
                dying."""
                """ "Please," he said. "Please help us." """
            ],
            [
                cb.Build("Of course I agreed.  Bandits have done me wrong many times on my travels.", "tez2-approve")
                cb.Build("Of course I agreed.  I could never leave a town without its Town Guardian.", "tez2-approve")
                cb.Build(
                    "I turned them down.  \"Why should I help the people who scattered mine to the wind?\"",
                    "tez2-deny")
            ])
        pb.Build(
            "tez2-deny",
            [
                """He was shocked. "You doom us," he cried. But I stood resolute, and after a while he calmed down."""
                """ "Would you do it for 50 muld shells?" He asked eventually.  In this region, that sum would have
                allowed someone to live comfortably for two months."""
            ],
            [
                cb.Build(
                    "That amount of muld shells wouldn't be easy to come by otherwise, so I agreed.",
                    "tez2-approve",
                    grantsMoney=50)
                cb.Build(
                    """It's not unheard of for towns to survive long enough to get a new Town Guardian. I was
                        unconvinced.""",
                    "tez2-denytwice")
            ])
        pb.Build(
            "tez2-denytwice",
            ["\"Seventy shells!\""],
            [cb.Build("That seemed reasonable to me, so I accepted.", "tez2-approve", grantsMoney=70)])
        pb.Build(
            "tez2-approve",
            [
                """He thanked me profusely.  He went out and brought more villagers back while I prepared my room, and
                when I was done half of the town was in the first floor.  They told me the story of how Greltza had used
                her [[Illusion Helm]] to take the form of a bumblebee, an animal beloved by the Guardian."""
                """The Town Guardian was not able to distinguish Greltza from her other bumblebees, and Greltza was able
                to steal the heartseed.  Greltza fled town to the east, heading towards the [[Ereshkigal Mountains]]. If
                she continued on that path, she would eventually cross into the [[Szaltun Desert]], before finally
                arriving at [[Estaton, the Inland Sea|Estaton]]."""
                """We spoke and planned long into the night, but I was eventually able to get rest, and my steed and I
                were refreshed in the morning when we set out.  I regretted not meeting the Town Guardian, but no
                outsiders were allowed to visit her in her weakened state."""
            ],
            [cb.Build(
                """As the sun rose, I headed along the Great Eastern Road, towards the forested foothills of the
                    mountains...""",
                "eresh-1")]
        )
        pb.Build(
            "middle",
            ["You made it closer..."],
            [cb.Build("Go easter", "end", SkillCheckRequired (Will, Persuasion, 30, AlternatePage "middle2"))])
        pb.Build(
            "middle2",
            ["You made it closer, but then you fucked up."],
            [
                cb.Build("Go eastest", "middle2", SkillCheckRequired (Might, Combat, 20, AttributeDamage), ["YouFuckedItUpViolently"])
                cb.Build("Go eastest, but pay", "middle2", Bribe 15, ["YouFuckedItUpPayedly"])
                cb.Build("Beat the game already!", "middle3", Flags (["YouFuckedItUpViolently"; "YouFuckedItUpPayedly"], Automatic))
            ])
        pb.Build("middle3", ["This'll reset you."], [cb.Build("Fine, just end it!", "end")], resets=true)
        pb.Build("end", ["You beat the game!"], [])
    ]
    |> List.map (fun p -> (p.Name, p))
    |> Map.ofList

let allRoutedPages =
    pages
    |> Seq.map (fun kvp -> kvp.Value)
    |> Seq.collect (fun page ->
        page.Continuations)
    |> Seq.choose (fun cont ->
        let rec getConditionPage cond =
            match cond with
            | Automatic -> Some cont.NextPageName
            | SkillCheckRequired (_, _, _, effect) ->
                match effect with
                | AlternatePage page -> Some page
                | AttributeDamage -> None
            | Flags (flags, c) -> getConditionPage c
            | _ -> None
        getConditionPage cont.Condition)
    |> Set.ofSeq

for kvp in pages do
    if not (Set.contains kvp.Value.Name allRoutedPages) && kvp.Value.Name <> "start" then
        printfn "Page %A is not reachable from any other pages" kvp.Value
    for continuation in kvp.Value.Continuations do
        if not (Map.containsKey continuation.NextPageName pages) then
            printfn "Page %A has an invalid continuation: %s" kvp.Value continuation.NextPageName
        match continuation.Condition with
        | SkillCheckRequired (attr, skill, target, effect) ->
            match effect with
            | AlternatePage name ->
            if not (Map.containsKey name pages) then
                    printfn "Continuation %A has an invalid alternate room." continuation
            | AttributeDamage -> ()
        | _ -> ()
    for part in List.collect Modals.parseLine kvp.Value.Text do
        match part with
        | Modals.Str _ -> ()
        | Modals.Link link ->
            if not (Map.containsKey link.LinkName Modals.modals) then
                printfn "Room %A has an invalid modal link: %s" kvp.Value link.LinkName