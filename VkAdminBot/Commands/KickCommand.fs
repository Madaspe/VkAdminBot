module VkAdminBot.Commands.Kick

open VkAdminBot.Methods.VkApiMethods
open VkNet
open VkNet.Model.GroupUpdate

let kickCommand (update: GroupUpdate) vkApi =
    sendMessage "Todo" update.MessageNew.Message.PeerId vkApi

