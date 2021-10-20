module VkAdminBot.Methods.VkApiMethods

open VkAdminBot.Methods.Utils
open System.Threading
open System.Threading.Channels
open VkNet.Extensions.Polling.Models.Configuration
open VkNet.Extensions.Polling
open VkNet.Model
open VkNet.Model.GroupUpdate
open VkNet
open FSharp.Control
open System

let random = new Random()

let sendMessage (text: string) peerId (vkApi: VkApi) =
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