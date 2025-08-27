/**
 * Review and Comment Management for Property Detail Page
 */

class ReviewManager {
    constructor() {
        this.currentPage = 1;
        this.pageSize = 10;
        this.totalComments = 0;
        this.comments = [];
        this.init();
    }

    init() {
        this.bindEvents();
        this.loadComments();
    }

    bindEvents() {
        // Comment form submission
        const commentForm = document.getElementById('commentForm');
        if (commentForm) {
            commentForm.addEventListener('submit', (e) => this.handleCommentSubmit(e));
        }

        // Star rating hover effects
        this.setupStarRating();
    }

    setupStarRating() {
        const starLabels = document.querySelectorAll('.star-label');
        //const starInputs = document.querySelectorAll('.star-input');

        starLabels.forEach((label, index) => {
            label.addEventListener('mouseenter', () => {
                //console.log(starLabels.length,index);
                this.highlightStars(index + 1);
            });

            label.addEventListener('mouseleave', () => {
                this.resetStarHighlight();
            });

            label.addEventListener('click', () => {
                const rating = index + 1;
                //const rating = starLabels.length - index;
                this.selectRating(rating);
            });
        });
    }

    highlightStars(count) {
        const starLabels = document.querySelectorAll('.star-label');
        starLabels.forEach((label, index) => {
            if (index < count) {
                label.style.color = '#fbbf24';
            } else {
                label.style.color = '#d1d5db';
            }
        });
    }

    resetStarHighlight() {
        const starLabels = document.querySelectorAll('.star-label');
        const selectedRating = document.querySelector('input[name="rating"]:checked');

        if (selectedRating) {
            this.highlightStars(parseInt(selectedRating.value));
        } else {
            starLabels.forEach(label => {
                label.style.color = '#d1d5db';
            });
        }
    }

    selectRating(rating) {
        const starInput = document.getElementById(`star${rating}`);
        if (starInput) {
            starInput.checked = true;
            this.highlightStars(rating);
        }
    }

    async handleCommentSubmit(e) {
        e.preventDefault();
        //console.log('selectedRating', selectedRating);
        const rating = document.querySelector('input[name="rating"]:checked');
        //const rating = selectedRating;
        const content = document.getElementById('commentContent').value.trim();

        if (!rating) {
            alert('Vui lòng chọn đánh giá sao!');
            return;
        }

        if (!content) {
            alert('Vui lòng nhập nội dung bình luận!');
            return;
        }

        try {
            const commentData = {
                propertyPostId: window.currentPropertyId,
                rating: parseInt(rating.value),
                reviewText: content
            };

            const result = await addComment(commentData);

            if (result) {
                if (result != 'Thành công') {
                    this.showMessage(result, 'error');
                    return;
                }
                // Reset form
                document.getElementById('commentForm').reset();
                this.resetStarHighlight();

                // Reload comments
                this.loadComments();

                // Show success message
                this.showMessage('Đánh giá của bạn đã được gửi thành công!', 'success');
            }
        } catch (error) {
            console.error('Error submitting comment:', error);
            this.showMessage('Có lỗi xảy ra khi gửi đánh giá. Vui lòng thử lại!', 'error');
        }
    }

    async loadComments() {
        try {
            const commentsData = await getComment(window.currentPropertyId);

            // Handle different response formats
            let rawComments = [];
            if (commentsData && commentsData.data) {
                rawComments = Array.isArray(commentsData.data) ? commentsData.data : [];
            } else if (commentsData && Array.isArray(commentsData)) {
                rawComments = commentsData;
            } else if (commentsData && commentsData.items) {
                rawComments = Array.isArray(commentsData.items) ? commentsData.items : [];
            }

            // Transform API data to expected format
            this.comments = rawComments.map(comment => this.transformCommentData(comment));

            this.totalComments = this.comments.length;

            this.displayComments();
            this.updateCommentCount();
        } catch (error) {
            console.error('Error loading comments:', error);
            this.showNoComments();
        }
    }

    // Transform API comment data to expected format
    transformCommentData(apiComment) {
        // Transform replies if they exist
        let transformedReplies = [];

        // Check for single reply object (API format: reply: {...})
        if (apiComment.reply && typeof apiComment.reply === 'object') {
            transformedReplies = [{
                id: apiComment.reply.id || 'unknown',
                userName: 'Chủ nhà', // Since it's landlordId, we'll use a default name
                content: apiComment.reply.replyContent || 'Không có nội dung',
                createdAt: apiComment.reply.createdAt
            }];
        }
        // Check for replies array (fallback format)
        else if (apiComment.replies && Array.isArray(apiComment.replies)) {
            transformedReplies = apiComment.replies.map(reply => ({
                id: reply.replyId || reply.id,
                userName: reply.userName || reply.renterName || reply.ownerName || 'Người dùng',
                content: reply.content || reply.replyText || reply.replyContent || 'Không có nội dung',
                createdAt: reply.createdAt
            }));
        }

        const transformedComment = {
            id: apiComment.reviewId || apiComment.id,
            rating: apiComment.rating || 0,
            content: apiComment.reviewText || apiComment.content || 'Không có nội dung',
            userName: apiComment.renterName || apiComment.userName || 'Người dùng',
            createdAt: apiComment.createdAt,
            replies: transformedReplies
        };

        return transformedComment;
    }

    displayComments() {
        const commentsList = document.getElementById('commentsList');
        const noComments = document.getElementById('noComments');
        const commentsLoading = document.querySelector('.comments-loading');

        if (!commentsList) return;

        // Hide loading
        if (commentsLoading) {
            commentsLoading.style.display = 'none';
        }

        if (this.comments.length === 0) {
            this.showNoComments();
            return;
        }

        // Show comments
        if (noComments) {
            noComments.classList.add('hidden');
        }

        const startIndex = (this.currentPage - 1) * this.pageSize;
        const endIndex = startIndex + this.pageSize;
        const pageComments = this.comments.slice(startIndex, endIndex);
        //document.getElementById('totalCommentCount').innerHTML = this.pageComments.length;
        const commentsHTML = pageComments.map(comment => this.renderComment(comment)).join('');
        commentsList.innerHTML = commentsHTML;

        // Show pagination if needed
        this.updatePagination();
    }

    createCommentHTML(name, text, stars, time) {
        const starsDisplay = '★'.repeat(stars) + ' ☆'.repeat(5 - stars);
        const comment = document.createElement('div');
        comment.className = 'flex space-x-4 items-start';

        comment.innerHTML = `
            <img src="https://i.pravatar.cc/48?u=${Math.random()}" alt="avatar" class="w-12 h-12 rounded-full shadow-sm">
            <div class="flex-1">
              <div class="bg-gray-50 p-4 rounded-xl shadow-sm hover:shadow transition">
                <div class="flex items-center justify-between mb-1">
                  <span class="font-semibold text-gray-800">${name}</span>
                  <span class="text-sm text-gray-500">${time}</span>
                </div>
                <div class="flex mb-2">
                  <span class="text-yellow-400">${starsDisplay}</span>
                </div>
                <p class="text-gray-700">${text}</p>
                <button class="text-sm text-blue-500 mt-2 hover:underline replyBtn">Trả lời</button>
                <button class="text-sm text-red-500 hover:underline report-btn">Báo cáo</button>
                <div class="replies space-y-3 mt-3 pl-8 border-l border-gray-200"></div>
              </div>
            </div>
          `;


        const replyBtn = comment.querySelector('.replyBtn');
        const repliesContainer = comment.querySelector('.replies');

        replyBtn.addEventListener('click', () => {
            if (comment.querySelector('.replyForm')) return;

            const replyForm = document.createElement('form');
            replyForm.className = 'replyForm mt-3';
            replyForm.innerHTML = `
              <textarea placeholder="Viết trả lời..."
                class="w-full p-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
                rows="2"></textarea>
              <button type="submit"
                class="mt-2 bg-green-500 hover:bg-green-600 text-white text-sm px-4 py-1 rounded-lg">
                Gửi trả lời
              </button>
            `;

            replyForm.addEventListener('submit', (e) => {
                e.preventDefault();
                const replyText = replyForm.querySelector('textarea').value.trim();
                if (replyText) {
                    const reply = document.createElement('div');
                    reply.className = 'flex space-x-3 items-start';
                    reply.innerHTML = `
                  <img src="https://i.pravatar.cc/36?u=${Math.random()}" alt="avatar" class="w-9 h-9 rounded-full shadow-sm">
                  <div class="bg-white p-3 rounded-lg shadow-sm flex-1">
                    <div class="font-semibold text-sm">Người trả lời <span class="text-gray-500 text-xs">vừa xong</span></div>
                    <p class="text-gray-700 text-sm">${replyText}</p>
                  </div>
                `;
                    repliesContainer.appendChild(reply);
                    replyForm.remove();
                }
            });

            repliesContainer.appendChild(replyForm);
        });

        return comment;
    }

    renderComment(comment) {
        // Validate comment data
        if (!comment || typeof comment !== 'object') {
            return '';
        }
        // Safe data extraction with fallbacks
        const rating = comment.rating || 0;
        const content = comment.content || 'Không có nội dung';
        const userName = comment.userName || comment.userName || 'Người dùng';
        const commentId = comment.id || 'unknown';

        // Safe date handling
        let dateStr = 'Không xác định';
        try {
            if (comment.createdAt) {
                const date = new Date(comment.createdAt);
                if (!isNaN(date.getTime())) {
                    dateStr = date.toLocaleDateString('vi-VN');
                }
            }
        } catch (error) {
            console.warn('Error parsing comment date:', error);
        }
        const params = new URLSearchParams(window.location.search);
        const stars = this.renderStars(rating);

        let type = params.get('type');


        return `
            <div class="comment-item" data-comment-id="${commentId}">
                <div class="comment-header">
                    <div class="comment-user-info">
                        <div class="comment-avatar">
                            ${userName ? userName.charAt(0).toUpperCase() : 'U'}
                        </div>
                        <div class="comment-user-details">
                            <div class="comment-username">${userName}</div>
                            <div class="comment-date">${dateStr}</div>
                        </div>
                    </div>
                    <div class="comment-rating">
                        <div class="comment-stars">
                            ${stars}
                        </div>
                    </div>
                </div>
                <div class="comment-content">
                    ${content}
                </div>
                <div class="comment-actions ${type != 'lardlord' ? 'd-none' : ''}">
                    <button class="comment-action-btn" onclick="reviewManager.replyToComment('${commentId}')">
                        Trả lời
                    </button>
                    <button class="comment-action-btn" onclick="reviewManager.reportComment('${commentId}')">
                        Báo cáo
                    </button>
                </div>
                ${comment.replies && Array.isArray(comment.replies) && comment.replies.length > 0 ? this.renderReplies(comment.replies, commentId) : ''}
            </div>
        `;
    }

    renderStars(rating) {
        let starsHTML = '';
        for (let i = 1; i <= 5; i++) {
            if (i <= rating) {
                starsHTML += '<span class="comment-star">★</span>';
            } else {
                starsHTML += '<span class="comment-star empty">★</span>';
            }
        }
        return starsHTML;
    }

    async reportComment(commentId) {
        if (!confirm("Bạn có muốn report ?")) return;
        try {
            let urlReply = `http://194.233.81.64:5000/api/Review/reply/report?commentId=${commentId}`;
            const token = localStorage.getItem('authToken');
            if (!token) {
                window.location.href = '/Auth/Login';
                return;
            }
            //let dto = {};
            //dto['ReviewId'] = commentId;
            //console.log(dto);
            const response = await fetch(urlReply, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                //body: JSON.stringify(dto)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            alert(data);
            if (data.includes('thành công')) {
                window.location.reload();
            }
            return data;
        } catch (error) {
            console.error('Add comment error:', error);
            throw error;
        }
    }

    renderReplies(replies, commentId) {
        if (!Array.isArray(replies) || replies.length === 0) {
            return '';
        }

        const repliesHTML = replies.map(reply => {
            if (!reply || typeof reply !== 'object') {
                return '';
            }

            // Safe data extraction with multiple field names
            const userName = reply.userName || reply.renterName || reply.ownerName || 'Người dùng';
            const content = reply.content || reply.replyText || reply.replyContent || 'Không có nội dung';

            // Safe date handling
            let dateStr = 'Không xác định';
            try {
                if (reply.createdAt) {
                    const date = new Date(reply.createdAt);
                    if (!isNaN(date.getTime())) {
                        dateStr = date.toLocaleDateString('vi-VN');
                    }
                }
            } catch (error) {
                // Silent error handling
            }

            return `
                <div class="comment-reply">
                    <div class="comment-reply-header">
                        <div class="comment-reply-user">${userName}</div>
                        <div class="comment-reply-date">${dateStr}</div>
                    </div>
                    <div class="comment-reply-content">${content}</div>
                </div>
                <div class="reply-form d-none" id="reply-${reply.id}">
                    <form class="comment-reply-content" data-reply-for="${commentId}">
                        <textarea class="reply-text comment-textarea" value="${content}" placeholder="Nhập phản hồi...">${content}</textarea>
                        <div class="reply-actions">
                            <button class="comment-action-btn" type="button" onclick="reviewManager.submitReply('${commentId}',${reply.id})">Gửi</button>
                            <button class="comment-action-btn" type="button" onclick="reviewManager.cancelReply('${commentId}')">Hủy</button>
                        </div>
                    </form>
                </div>
            `;
        }).join('');
        return repliesHTML;
    }

    showNoComments() {
        const commentsList = document.getElementById('commentsList');
        const noComments = document.getElementById('noComments');
        const commentsLoading = document.querySelector('.comments-loading');

        if (commentsList) commentsList.innerHTML = '';
        if (commentsLoading) commentsLoading.style.display = 'none';
        if (noComments) noComments.classList.remove('hidden');
    }

    updateCommentCount() {
        const totalCommentCount = document.getElementById('totalCommentCount');
        const totalCommentCount1 = document.getElementById('totalCommentCount1');
        if (totalCommentCount && totalCommentCount1) {
            totalCommentCount.textContent = this.totalComments;
            totalCommentCount1.textContent = this.totalComments;
        }

        // Also update the header comment count if it exists
        const headerCommentCount = document.querySelector('.comment-count span span');
        if (headerCommentCount) {
            headerCommentCount.textContent = this.totalComments;
        }
    }

    updatePagination() {
        const totalPages = Math.ceil(this.totalComments / this.pageSize);
        const pagination = document.getElementById('commentsPagination');
        const prevBtn = document.getElementById('prevComment');
        const nextBtn = document.getElementById('nextComment');
        const pageInfo = document.getElementById('commentPageInfo');

        if (totalPages <= 1) {
            if (pagination) pagination.classList.add('hidden');
            return;
        }

        if (pagination) pagination.classList.remove('hidden');
        if (prevBtn) prevBtn.disabled = this.currentPage === 1;
        if (nextBtn) nextBtn.disabled = this.currentPage === totalPages;
        if (pageInfo) pageInfo.textContent = `Trang ${this.currentPage} / ${totalPages}`;

        // Bind pagination events
        if (prevBtn) {
            prevBtn.onclick = () => this.goToPage(this.currentPage - 1);
        }
        if (nextBtn) {
            nextBtn.onclick = () => this.goToPage(this.currentPage + 1);
        }
    }

    goToPage(page) {
        if (page < 1 || page > Math.ceil(this.totalComments / this.pageSize)) return;

        this.currentPage = page;
        this.displayComments();
    }

    async likeComment(commentId) {
        // Implement like functionality
        console.log('Liking comment:', commentId);
    }

    async replyToComment(commentId) {
        const actionsDivContent = document.querySelector(`.comment-item[data-comment-id="${commentId}"] .comment-reply`);
        const actionsDiv = document.querySelector(`.comment-item[data-comment-id="${commentId}"] .reply-form`);
        if (!actionsDiv) return;
        actionsDivContent.classList.add('d-none');
        actionsDiv.classList.remove('d-none');
    }

    async cancelReply(commentId) {
        const actionsDivContent = document.querySelector(`.comment-item[data-comment-id="${commentId}"] .comment-reply`);
        const actionsDiv = document.querySelector(`.comment-item[data-comment-id="${commentId}"] .reply-form`);
        if (!actionsDiv) return;
        actionsDiv.classList.add('d-none');
        actionsDivContent.classList.remove('d-none');
        $('.comment-textarea').val('');
    }

    async submitReply(commentId,replyId) {
        try {
            const textArea = document.querySelector(`.comment-item[data-comment-id="${commentId}"] .reply-form .comment-textarea`);
            const actionsDiv = document.querySelector(`.comment-item[data-comment-id="${commentId}"] .reply-form`);
            let urlReply = "http://194.233.81.64:5000/api/Review/reply";
            if (replyId != 0) urlReply += '/edit';
            const token = localStorage.getItem('authToken');
            if (!token) {
                window.location.href = '/Auth/Login';
                return;
            }
            let dto = {};
            //dto['ReplyContent'] = textArea.val();
            if (replyId == 0) {
                dto['ReviewId'] = commentId;      
            }
            else {
                dto['ReplyId'] = actionsDiv.id.split('-')[1];  
            }
            dto['ReplyContent'] = textArea.value; 
            const response = await fetch(urlReply, {
                method: replyId == 0 ? 'POST' : 'PUT',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(dto)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            alert(data);
            if (data.includes('thành công')) {
                window.location.reload();
            }
            return data;
        } catch (error) {
            console.error('Add comment error:', error);
            throw error;
        }
    }

    showMessage(message, type = 'info') {
        // Simple message display
        alert(message);
    }

    // Public method to force reload comments
    forceLoadComments() {
        this.loadComments();
    }
}

// Initialize review manager when DOM is loaded
let reviewManager;
document.addEventListener('DOMContentLoaded', () => {
    // Wait a bit for other scripts to load
    setTimeout(() => {
        reviewManager = new ReviewManager();
        window.reviewManager = reviewManager; // Make it globally accessible
        console.log('ReviewManager initialized:', reviewManager);
    }, 200);
});

// Legacy functions for backward compatibility
async function addComment(dto) {
    try {
        const token = localStorage.getItem('authToken');
        if (!token) {
            window.location.href = '/Auth/Login';
            return;
        }
        console.log(dto);

        const response = await fetch(`http://194.233.81.64:5000/api/Review/add`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(dto)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Add comment error:', error);
        throw error;
    }
}

async function listInterest() {
    const token = localStorage.getItem('authToken');
    try {
        if (!token) {
            window.location.href = '/Auth/Login';
            return;
        }

        let userId = 0;
        const payload = JSON.parse(atob(token.split('.')[1]));
        userId = payload.id;

        const response = await fetch(`http://194.233.81.64:5000/api/Property/InterestedProperty/ByRenter/${userId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('List interest error:', error);
        throw error;
    }
}

async function editReplyComment(dto) {
    try {
        const token = localStorage.getItem('authToken');
        if (!token) {
            window.location.href = '/Auth/Login';
            return;
        }

        const response = await fetch(`http://194.233.81.64:5000/api/Review/reply/edit`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(dto)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Edit reply error:', error);
        throw error;
    }
}

async function getComment(propertyId) {
    try {
        const response = await fetch(`http://194.233.81.64:5000/api/Review/post/${propertyId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Get comment error:', error);
        throw error;
    }
}