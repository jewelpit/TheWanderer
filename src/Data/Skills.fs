module Wanderer.Skills

open System

type Skill =
    | Persuasion
    | Combat
    | Ritual
    | Sneaking

type Attribute =
    | Might
    | Will

type RollResult = {
    Succeeded : bool
    Target : int
    Rolls : int list
    AttributeLevel : int
    SkillRank : int
}

let private random = Random()

let roll attribute skill target =
    let rolls = List.init skill (fun _ -> random.Next(6) + 1)
    printfn "Rolled %A" rolls
    {
        Succeeded = List.length (List.filter (fun r -> r <= attribute) rolls) >= target
        Target = target
        Rolls = rolls
        AttributeLevel = attribute
        SkillRank = skill
    }