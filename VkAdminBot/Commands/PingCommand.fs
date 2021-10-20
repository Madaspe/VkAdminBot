module VkAdminBot.Commands.Ping

open VkAdminBot.Methods.VkApiMethods
open VkNet
open VkNet.Model.GroupUpdate

let pingCommand (update: GroupUpdate) vkApi =
    sendMessage "Pong" update.MessageNew.Message.PeerId vkApi

