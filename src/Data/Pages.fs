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
    Description : ReactElement
    NextPageName : string
    Condition : Condition
    SetFlags : string list
}

type ContinuationBuilder() =
    member __.Build(description, nextPageName, ?condition, ?setFlags) =
        {
            Description = description
            NextPageName = nextPageName
            Condition = defaultArg condition Automatic
            SetFlags = defaultArg setFlags []
        }

type Page = {
    Name : string
    Text : string list
    Continuations : Continuation list
}

let private cb = ContinuationBuilder()

let pages =
    [
        {
            Name = "start"
            Text =
                [
                    """I first made my way into Tetznatalk as dusk was falling, after a long day of travel.  I was
                    weary, and looking forward to my first night indoors in nearly a month.  I rode up to the inn and
                    stabled my [[monitor beetle|Monitor Beetle]], relieved that the inn seemed to have facilities to care
                    for it."""
                    """When I walked into the bar, I saw a shaky, bespectacled Etzeznalt behind the counter, nervously
                    cleaning a glass.  He was dressed in the round black hat and brown kilt typical of rural
                    Etzneznalts, and looked lost in thought.  At the sound of my arrival he looked up, and before he
                    could stop blurted out, "Please!  You must help us!" """
                ]
            Continuations = [cb.Build(R.str "\"Excuse me?\"", "tez1"); cb.Build(R.str "Enter test harness", "middle")]
        }
        {
            Name = "middle"
            Text = ["You made it closer..."]
            Continuations =
                [cb.Build(R.str "Go easter", "end", SkillCheckRequired (Will, Persuasion, 30, AlternatePage "middle2"))]
        }
        {
            Name = "middle2"
            Text = ["You made it closer, but then you fucked up."]
            Continuations =
                [
                    cb.Build(R.str "Go eastest", "middle2", SkillCheckRequired (Might, Combat, 20, AttributeDamage), ["YouFuckedItUpViolently"])
                    cb.Build(R.str "Go eastest, but pay", "middle2", Bribe 15, ["YouFuckedItUpPayedly"])
                    cb.Build(R.str "Beat the game already!", "end", Flags (["YouFuckedItUpViolently"; "YouFuckedItUpPayedly"], Automatic))
                ]
        }
        {
            Name = "end"
            Text = ["You beat the game!"]
            Continuations = []
        }
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