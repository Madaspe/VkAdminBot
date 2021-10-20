module VkAdminBot.Methods.Utils

open VkNet.Model.GroupUpdate
open FSharp.Control
open System

let random = new Random()

let getEnvVar x =
    let envVars = 
        System.Environment.GetEnvironmentVariables()
        |> Seq.cast<System.Collections.DictionaryEntry>
        |> Seq.map (fun d -> d.Key :?> string, d.Value :?> string)
        |> Map.ofSeq

    envVars.Item(x)

let notNull value = not (obj.ReferenceEquals(value, null))

let messageText (update: GroupUpdate): string option =
    if notNull update.MessageNew then
        let text = update.MessageNew.Message.Text

        if notNull text then
            Some text
        else None
    else None