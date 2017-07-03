module Wanderer.Model

open Wanderer.Data

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

type Character = {
    Might : int
    Will : int
    Persuasion : int
    Combat : int
    Ritual : int
    Sneaking : int
}

type ActiveGameState = {
    Character : Character
    Page : Page
}

type SavedGameState = {
    Character : Character
    PageName : string
}

type Model =
    | CharacterCreation of InProgressCharacter
    | ActiveGame of ActiveGameState

type Message =
    | UpdateCharacter of InProgressCharacter
    | StartGame
    | Flip of string