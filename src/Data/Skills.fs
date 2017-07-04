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
    Target : int
    Rolls : int list
    Attribute : Attribute
    Skill : Skill
}

let private random = new Random()