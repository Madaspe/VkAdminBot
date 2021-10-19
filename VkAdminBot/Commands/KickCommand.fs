module VkAdminBot.Commands.Kick

open VkAdminBot.Methods
open VkNet
open VkNet.Model.GroupUpdate

let kickCommand (update: GroupUpdate) vkApi =
    sendMessage "Todo" update.MessageNew.Message.PeerId vkApi

