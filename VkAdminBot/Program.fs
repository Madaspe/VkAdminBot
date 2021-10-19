open System.Threading
open System.Threading.Channels
open VkNet.Extensions.Polling.Models.Configuration
open VkNet.Extensions.Polling
open VkNet.Model
open VkNet.Model.GroupUpdate
open VkNet
open FSharp.Control
open System

let getEnvVar x =
    let envVars = 
      System.Environment.GetEnvironmentVariables()
      |> Seq.cast<System.Collections.DictionaryEntry>
      |> Seq.map (fun d -> d.Key :?> string, d.Value :?> string)
      |> Map.ofSeq

    envVars.Item(x)

let groupToken = getEnvVar "TOKEN"

let vkApi = new VkApi ()

let authParams = new ApiAuthParams (AccessToken=groupToken)

let random = new Random()

let notNull value = not (obj.ReferenceEquals(value, null))

let sendMessage text peerId =
    let messageParams = new RequestParams.MessagesSendParams (
        Message=text,
        PeerId=peerId,
        RandomId=(int64 <| random.Next 999999)
    )
        
    vkApi.Messages.Send(messageParams) |> ignore

let messageText (update: GroupUpdate): string option =
    if notNull update.MessageNew then
        let text = update.MessageNew.Message.Text

        if notNull text then
            Some text
        else None
    else None

let pingCommand (update: GroupUpdate) =
    sendMessage "Pong" update.MessageNew.Message.PeerId

let echoCommand (update: GroupUpdate) =
     sendMessage (messageText update).Value update.MessageNew.Message.PeerId

let commandDispatch (update: GroupUpdate) =
    let text = (messageText update).Value

    let messageList: string list =
        text.Split [|' '|]
        |> List.ofArray

    match messageList with
    | ["!ping"] -> pingCommand update
    | _ -> echoCommand update

let updateDispatch (update) =
    let text = messageText update

    match text with
    | Some(text) -> commandDispatch update
    | None -> ()


let readChannel (channelReader: ChannelReader<_>) =
    channelReader.ReadAllAsync()
    |> AsyncSeq.ofAsyncEnum
    |> AsyncSeq.iter (fun (update: GroupUpdate) -> updateDispatch update)


[<EntryPoint>]
let main args =
    let exitCode = 0

    let cancellationTokenSource = new CancellationTokenSource()

    vkApi.Authorize authParams |> ignore

    let groupLongPoll = vkApi.StartGroupLongPollAsync(GroupLongPollConfiguration.Default, cancellationTokenSource.Token)
    let channelReader = groupLongPoll.AsChannelReader()

    while true do
        readChannel channelReader |> Async.RunSynchronously |> ignore

    exitCode



                        
