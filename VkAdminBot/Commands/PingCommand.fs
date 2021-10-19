module VkAdminBot.Commands.PingCommand

open VkAdminBot.Methods
open VkNet
open VkNet.Model.GroupUpdate


let pingCommand (update: GroupUpdate) =
    sendMessage "Pong" update.MessageNew.Message.PeerId

