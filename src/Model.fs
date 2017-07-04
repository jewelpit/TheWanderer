module rec Wanderer.Model

open Fable.Import.React

open Wanderer.Modals
open Wanderer.Pages

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

type Message =
    | StartCharacterCreation
    | UpdateCharacter of InProgressCharacter
    | StartGame
    | LoadGame of ActiveGameState
    | Flip of string
    | ShowModal of Modal
    | CloseModal

type Model =
    | SplashScreen
    | CharacterCreation of InProgressCharacter
    | ActiveGame of ActiveGameState
    | Modal of Modal * Model