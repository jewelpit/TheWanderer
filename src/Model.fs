module Wanderer.Model

type Skill =
    | Persuasion
    | Combat
    | Ritual
    | Sneaking

type InProgressCharacter = {
    Might : int
    Will : int
    HighSkill : Skill
    LowSkill : Skill
}

type Model =
    | CharacterCreation of InProgressCharacter

type Message =
    | UpdateCharacter of InProgressCharacter