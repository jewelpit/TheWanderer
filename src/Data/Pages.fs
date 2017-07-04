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

type Continuation = {
    Description : ReactElement
    NextPageName : string
    Condition : Condition
}

type Page = {
    Name : string
    Text : string list
    Continuations : Continuation list
}

let pages =
    [
        {
            Name = "start"
            Text = ["""I first made my way into Tetznatalk as dusk was falling, after a long day of travel. There was an
                [[etzeznalt|Etzeznalt]] there."""]
            Continuations = [{ Description = R.str "Go east"; NextPageName = "middle"; Condition = Automatic }]
        }
        {
            Name = "middle"
            Text = ["You made it closer..."]
            Continuations =
                [
                    {
                        Description = R.str "Go easter"
                        NextPageName = "end"
                        Condition = SkillCheckRequired (Will, Persuasion, 30, AlternateRoom "middle2")
                    }
                ]
        }
        {
            Name = "middle2"
            Text = ["You made it closer, but then you fucked up."]
            Continuations =
                [
                    {
                        Description = R.str "Go eastest"
                        NextPageName = "middle2"
                        Condition = SkillCheckRequired (Might, Combat, 20, AttributeDamage)
                    }
                    {
                        Description = R.str "Go eastest, but pay"
                        NextPageName = "middle2"
                        Condition = Bribe 15
                    }
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