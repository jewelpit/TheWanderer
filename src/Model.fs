module rec Wanderer.Model

open Fable.Import.React

type Continuation = {
    Description : ReactElement
    NextPageName : string
}

type Page = {
    Name : string
    Text : (Message -> unit) -> ActiveGameState -> ReactElement
    Continuations : Continuation list
}

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
    | ShowModal of string * ReactElement
    | CloseModal

type Model =
    | SplashScreen
    | CharacterCreation of InProgressCharacter
    | ActiveGame of ActiveGameState
    | Modal of string * ReactElement * Model