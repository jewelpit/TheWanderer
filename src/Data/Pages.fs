module Wanderer.Pages

open Fable.Import.React

open Wanderer.Skills

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

type FailureEffect =
    | AlternateRoom of string
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
            Text = ["""I first made my way into Tetznatalk as dusk was falling, after a long day of travel. There was an
                [[etzeznalt|Etzeznalt]] there."""]
            Continuations = [cb.Build(R.str "Go east", "middle")]
        }
        {
            Name = "middle"
            Text = ["You made it closer..."]
            Continuations =
                [cb.Build(R.str "Go easter", "end", SkillCheckRequired (Will, Persuasion, 30, AlternateRoom "middle2"))]
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

for kvp in pages do
    for continuation in kvp.Value.Continuations do
        if not (Map.containsKey continuation.NextPageName pages) then
            printfn "Room %A has an invalid continuation: %s" kvp.Value continuation.NextPageName
        match continuation.Condition with
        | SkillCheckRequired (attr, skill, target, effect) ->
            match effect with
            | AlternateRoom name ->
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