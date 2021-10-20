module VkAdminBot.Methods.Utils

let getEnvVar x =
    let envVars = 
        System.Environment.GetEnvironmentVariables()
        |> Seq.cast<System.Collections.DictionaryEntry>
        |> Seq.map (fun d -> d.Key :?> string, d.Value :?> string)
        |> Map.ofSeq

    envVars.Item(x)

let notNull value = not (obj.ReferenceEquals(value, null))