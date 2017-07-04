module rec Wanderer.Model

open Fable.Import.React

open Wanderer.Modals
open Wanderer.Pages
open Wanderer.Skills

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
with
    static member GetAttr attr character =
        match attr with
        | Might -> character.Might
        | Will -> character.Will

    static member GetSkill skill character =
        match skill with
        | Persuasion -> character.Persuasion
        | Combat -> character.Combat
        | Ritual -> character.Ritual
        | Sneaking -> character.Sneaking

type ActiveGameState = {
    Character : Character
    Page : Page
    History : string list
}

type SavedGameState = {
    Character : Character
    PageName : string
    History : string list
}

type Message =
    | StartCharacterCreation
    | UpdateCharacter of InProgressCharacter
    | StartGame
    | LoadGame of ActiveGameState
    | Flip of Continuation
    | ShowModal of Modal
    | CloseModal

type Model =
    | SplashScreen
    | CharacterCreation of InProgressCharacter
    | ActiveGame of ActiveGameState
    | Modal of Modal * Model