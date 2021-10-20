module VkAdminBot.Methods.VkApiMethods

open VkAdminBot.Methods.Utils
open VkNet.Model
open VkNet

let sendMessage (text: string) peerId (vkApi: VkApi) =
    let messageParams = new RequestParams.MessagesSendParams (
        Message=text,
        PeerId=peerId,
        RandomId=(int64 <| random.Next 999999)
    )
            
    vkApi.Messages.Send(messageParams) |> ignore
       