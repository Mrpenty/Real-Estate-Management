// Support Chat functionality
let currentSupportConversationId = null;
let supportSignalRConnection = null;

function openSimpleSupportChat() {
    console.log('openSimpleSupportChat called'); // Debug log
    console.log('authService.isAuthenticated():', authService.isAuthenticated());
    console.log('localStorage authToken:', localStorage.getItem('authToken') ? 'EXISTS' : 'NOT EXISTS');
    
    if (!authService.isAuthenticated()) {
        Swal.fire({
            icon: 'info',
            title: 'Vui lòng đăng nhập',
            text: 'Bạn cần đăng nhập để có thể liên hệ hỗ trợ.',
            confirmButtonText: 'Đăng nhập',
            showCancelButton: true,
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = '/Auth/Login';
            }
        });
        return;
    }
    
    // Open simple chat modal
    const modal = document.getElementById('simpleSupportChatModal');
    if (modal) {
        modal.classList.remove('hidden');
        console.log('Modal opened successfully'); // Debug log
        
        // Focus on input
        setTimeout(() => {
            const input = document.getElementById('simpleChatMessageInput');
            if (input) {
                input.focus();
                console.log('Input focused'); // Debug log
            }
        }, 100);
        
        // Load existing conversation if any
        loadSimpleSupportConversation();
    } else {
        console.error('Modal not found!'); // Debug log
        // Fallback to redirect
        window.location.href = '/Support/Index';
    }
}

function closeSimpleSupportChatModal() {
    document.getElementById('simpleSupportChatModal').classList.add('hidden');
    document.getElementById('simpleChatMessageInput').value = '';
}

async function loadSimpleSupportConversation() {
    try {
        const token = localStorage.getItem('authToken');
        if (!token) return;

        const response = await fetch(config.buildApiUrl(config.chat.userSupportConversations), {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const conversations = await response.json();
            if (conversations.length > 0) {
                // Use the first conversation
                const conversation = conversations[0];
                await loadSimpleSupportMessages(conversation.id);
            }
        }
    } catch (error) {
        console.error('Error loading conversation:', error);
    }
}

async function loadSimpleSupportMessages(conversationId) {
    console.log('🔄 Loading support messages for conversation:', conversationId);
    try {
        const token = localStorage.getItem('authToken');
        if (!token) return;

        const response = await fetch(config.buildApiUrl(`${config.chat.supportMessages}/${conversationId}/messages`), {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const messages = await response.json();
            console.log('📨 Loaded messages:', messages.length, 'messages');
            displaySimpleSupportMessages(messages);
        }
    } catch (error) {
        console.error('Error loading messages:', error);
    }
}

function displaySimpleSupportMessages(messages) {
    console.log('🎨 Displaying support messages:', messages.length, 'messages');
    const container = document.getElementById('simpleChatMessages');
    let html = '';
    
    if (messages.length === 0) {
        html = '<div class="text-center text-gray-500 py-8"><i class="fas fa-comments text-4xl mb-2 text-gray-300"></i><p>Bắt đầu cuộc trò chuyện với admin</p></div>';
    } else {
        messages.forEach(message => {
            const isFromAdmin = message.isFromAdmin;
            const messageClass = isFromAdmin ? 'bg-blue-100 ml-8' : 'bg-white mr-8';
            const time = new Date(message.sentAt).toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
            const senderName = isFromAdmin ? 'Admin' : 'Bạn';
            
            html += `
                <div class="mb-3 ${isFromAdmin ? 'text-right' : 'text-left'}">
                    <div class="inline-block ${messageClass} rounded-lg px-3 py-2 max-w-xs lg:max-w-md">
                        <div class="text-sm">${message.content}</div>
                        <div class="text-xs text-gray-500 mt-1">${time}</div>
                    </div>
                    <div class="text-xs text-gray-400 mt-1">${senderName}</div>
                </div>
            `;
        });
    }
    
    container.innerHTML = html;
    container.scrollTop = container.scrollHeight;
    console.log('✅ Messages displayed successfully');
}

async function sendSimpleSupportMessage() {
    const input = document.getElementById('simpleChatMessageInput');
    const content = input.value.trim();
    
    if (!content) {
        showSimpleErrorMessage('Vui lòng nhập nội dung tin nhắn');
        return;
    }

    try {
        const token = localStorage.getItem('authToken');
        if (!token) {
            showSimpleErrorMessage('Vui lòng đăng nhập lại');
            return;
        }

        // Get or create conversation
        console.log('Getting or creating conversation...');
        let conversationId = await getOrCreateSimpleConversation();
        console.log('Conversation ID result:', conversationId);
        
        if (!conversationId) {
            showSimpleErrorMessage('Không thể tạo cuộc trò chuyện');
            return;
        }

        // Send message
        const sendMessageUrl = config.buildApiUrl(`${config.chat.sendSupportMessage}/${conversationId}/send-message`);
        console.log('Sending message to URL:', sendMessageUrl);
        console.log('Message content:', content);
        
        const response = await fetch(sendMessageUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(content)
        });

        console.log('Send message response status:', response.status);
        console.log('Send message response ok:', response.ok);

                        if (response.ok) {
                    input.value = '';
                    await loadSimpleSupportMessages(conversationId);
                    // Bỏ thông báo thành công
                } else {
            const error = await response.text();
            console.error('Send message error:', error);
            showSimpleErrorMessage('Lỗi: ' + error);
        }
    } catch (error) {
        console.error('Error sending message:', error);
        showSimpleErrorMessage('Lỗi kết nối: ' + error.message);
    }
}

async function getOrCreateSimpleConversation() {
    try {
        const token = localStorage.getItem('authToken');
        console.log('getOrCreateSimpleConversation - token exists:', !!token);
        if (!token) {
            console.error('No auth token found in localStorage');
            return null;
        }

        // Check if user has existing conversation
        console.log('Checking existing conversations...');
        const response = await fetch(config.buildApiUrl(config.chat.userSupportConversations), {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const conversations = await response.json();
            if (conversations.length > 0) {
                return conversations[0].id;
            }
        }

        // Create new conversation
        console.log('Creating support conversation...');
        
        const requestBody = {
            initialMessage: 'Bắt đầu cuộc trò chuyện'
        };
        
        const createResponse = await fetch(config.buildApiUrl(config.chat.createSupportConversation), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(requestBody)
        });

        if (createResponse.ok) {
            const result = await createResponse.json();
            console.log('Support conversation created successfully:', result.conversationId);
            return result.conversationId;
        } else {
            const errorText = await createResponse.text();
            console.error('API Error:', errorText);
        }
    } catch (error) {
        console.error('Error getting/creating conversation:', error);
    }
    return null;
}

//function showSimpleSuccessMessage(message) {
//    if (window.Swal) {
//        Swal.fire({
//            icon: 'success',
//            title: 'Thành công!',
//            text: message,
//            timer: 2000,
//            showConfirmButton: false
//        });
//    } else {
//        alert(message);
//    }
//}

function showSimpleErrorMessage(message) {
    if (window.Swal) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: message,
            confirmButtonText: 'Đóng'
        });
    } else {
        alert(message);
    }
}

// Initialize SignalR connection for support chat
function initializeSupportSignalR() {
    if (!authService.isAuthenticated()) return;
    
    try {
        const token = localStorage.getItem('authToken');
        supportSignalRConnection = new signalR.HubConnectionBuilder()
            .withUrl('http://194.233.81.64:5000/chatHub', { accessTokenFactory: () => token })
            .withAutomaticReconnect()
            .build();

        supportSignalRConnection.start()
            .then(() => {
                console.log('Support Chat SignalR connected');
                setupSupportSignalRHandlers();
            })
            .catch(err => {
                console.error('Support Chat SignalR error:', err);
            });

        // Handle connection errors
        supportSignalRConnection.onclose((error) => {
            console.log('Support Chat SignalR connection closed:', error);
        });

    } catch (error) {
        console.error('Error creating Support Chat SignalR connection:', error);
    }
}

// Setup SignalR event handlers for support chat
function setupSupportSignalRHandlers() {
    if (!supportSignalRConnection) return;

    // Listen for new support messages
    supportSignalRConnection.on('SupportMessageReceived', (data) => {
        console.log('New support message received:', data);
        
        // Update unread count
        checkUnreadSupportMessages();
        
        // Always refresh messages if chat is open, regardless of conversation ID
        if (currentSupportConversationId) {
            console.log('🔄 Auto-refreshing support chat messages for conversation:', currentSupportConversationId);
            loadSimpleSupportMessages(currentSupportConversationId);
        }
    });

    // Listen for support status updates
    supportSignalRConnection.on('SupportStatusUpdated', (data) => {
        console.log('Support status updated:', data);
        // Could show notification here
    });
}

// Check for unread support messages
async function checkUnreadSupportMessages() {
    if (!authService.isAuthenticated()) return;
    
    try {
        const token = localStorage.getItem('authToken');
        const response = await fetch(config.buildApiUrl(config.chat.userSupportConversations), {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const conversations = await response.json();
            const totalUnread = conversations.reduce((sum, conv) => sum + (conv.unreadCount || 0), 0);
            
            // Update unread badge
            const unreadBadge = document.getElementById('supportUnreadBadge');
            if (unreadBadge) {
                if (totalUnread > 0) {
                    unreadBadge.textContent = totalUnread > 99 ? '99+' : totalUnread;
                    unreadBadge.classList.remove('hidden');
                    
                    // Add pulse animation for new messages
                    unreadBadge.classList.add('animate-pulse');
                    setTimeout(() => {
                        unreadBadge.classList.remove('animate-pulse');
                    }, 2000);
                } else {
                    unreadBadge.classList.add('hidden');
                }
            }
        }
    } catch (error) {
        console.error('Error checking unread support messages:', error);
    }
}

// Initialize support chat when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    console.log('SupportChat DOM loaded');
    console.log('Config available:', typeof config !== 'undefined');
    if (typeof config !== 'undefined') {
        console.log('Config object:', config);
        console.log('Config chat object:', config.chat);
    }
    
    // Initialize SignalR connection for support chat
    initializeSupportSignalR();
    
    // Add Enter key support for simple chat
    const simpleChatInput = document.getElementById('simpleChatMessageInput');
    if (simpleChatInput) {
        simpleChatInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                sendSimpleSupportMessage();
            }
        });
    }
    
    // Close modal when clicking outside
    const simpleModal = document.getElementById('simpleSupportChatModal');
    if (simpleModal) {
        simpleModal.addEventListener('click', function(e) {
            if (e.target === simpleModal) {
                closeSimpleSupportChatModal();
            }
        });
    }
    
    // Close modal with Escape key
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            const modal = document.getElementById('simpleSupportChatModal');
            if (modal && !modal.classList.contains('hidden')) {
                closeSimpleSupportChatModal();
            }
        }
    });
    
    // Check for unread messages
    checkUnreadSupportMessages();
}); 