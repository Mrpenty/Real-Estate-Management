namespace RealEstateManagement.API.Hubs
{
    public static class ChatConnectionManager
    {
        private static readonly Dictionary<string, HashSet<int>> _conversationUsers = new();
        // (userId -> conversationId) ⬅️ mới thêm
        private static readonly Dictionary<int, string> _userToConversation = new();
        public static void AddUser(string conversationId, int userId)
        {
            // ❗ Nếu user đang ở conversation khác → remove
            if (_userToConversation.TryGetValue(userId, out var oldConversationId))
            {
                if (_conversationUsers.ContainsKey(oldConversationId))
                {
                    _conversationUsers[oldConversationId].Remove(userId);
                    if (_conversationUsers[oldConversationId].Count == 0)
                        _conversationUsers.Remove(oldConversationId);
                }
            }

            // ✅ Thêm vào conversation mới
            if (!_conversationUsers.ContainsKey(conversationId))
            {
                _conversationUsers[conversationId] = new HashSet<int>();
            }

            _conversationUsers[conversationId].Add(userId);
            _userToConversation[userId] = conversationId;
        }


        public static void RemoveUser(string conversationId, int userId)
        {
            if (_conversationUsers.ContainsKey(conversationId))
            {
                _conversationUsers[conversationId].Remove(userId);
                if (_conversationUsers[conversationId].Count == 0)
                    _conversationUsers.Remove(conversationId);
            }

            if (_userToConversation.ContainsKey(userId))
                _userToConversation.Remove(userId);
        }

        public static HashSet<int> GetConnections(string conversationId)
        {
            if (_conversationUsers.ContainsKey(conversationId))
                return _conversationUsers[conversationId];
            return new HashSet<int>();
        }
        public static void RemoveUserFromAllConversations(int userId)
        {
            if (_userToConversation.TryGetValue(userId, out var conversationId))
            {
                RemoveUser(conversationId, userId);
            }
        }

    }

}
