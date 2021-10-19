namespace VkAdminBot

open VkAdminBot.Methods
open System.Threading
open System.Threading.Channels
open VkNet.Extensions.Polling.Models.Configuration
open VkNet.Extensions.Polling
open VkNet.Model
open VkNet.Model.GroupUpdate
open VkNet
open FSharp.Control
open System

module Program =
    let groupToken = getEnvVar "TOKEN"

    let vkApi = new VkApi ()

    let authParams = new ApiAuthParams (AccessToken=groupToken)

    let commandDispatch (update: GroupUpdate) =
        let text = (messageText update).Value

        let messageList: string list =
            text.Split [|' '|]
            |> List.ofArray

        match messageList with
        | ["!ping"] -> VkAdminBot.Commands.Ping.pingCommand update vkApi
        | _ -> ()

    let updateDispatch (update) =
        let text = messageText update

        match text with
        | Some(text) -> commandDispatch update
        | None -> ()

    let groupLongPoll (token: CancellationTokenSource) =
        vkApi.StartGroupLongPollAsync(GroupLongPollConfiguration.Default, token.Token)

    let readChannel (channelReader: ChannelReader<_>) =
        channelReader.ReadAllAsync()
        |> AsyncSeq.ofAsyncEnum
        |> AsyncSeq.iter (fun (update: GroupUpdate) -> updateDispatch update)


    [<EntryPoint>]
    let main args =
        let exitCode = 0

        let cancellationTokenSource = new CancellationTokenSource ()

        vkApi.Authorize authParams |> ignore

        let channelReader =
            groupLongPoll cancellationTokenSource
            |> fun x -> x.AsChannelReader()

        while true do
            readChannel channelReader |> Async.RunSynchronously |> ignore

        exitCode



                            
