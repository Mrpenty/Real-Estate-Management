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
        const starInputs = document.querySelectorAll('.star-input');

        starLabels.forEach((label, index) => {
            label.addEventListener('mouseenter', () => {
                this.highlightStars(starLabels.length - index);
            });

            label.addEventListener('mouseleave', () => {
                this.resetStarHighlight();
            });

            label.addEventListener('click', () => {
                const rating = starLabels.length - index;
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
        
        const rating = document.querySelector('input[name="rating"]:checked');
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
                propertyId: window.currentPropertyId,
                rating: parseInt(rating.value),
                content: content
            };

            const result = await addComment(commentData);
            
            if (result) {
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

        const commentsHTML = pageComments.map(comment => this.renderComment(comment)).join('');
        commentsList.innerHTML = commentsHTML;

        // Show pagination if needed
        this.updatePagination();
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
        
        const stars = this.renderStars(rating);
        
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
                <div class="comment-actions">
                    <button class="comment-action-btn" onclick="reviewManager.likeComment('${commentId}')">
                        👍 Thích
                    </button>
                    <button class="comment-action-btn" onclick="reviewManager.replyToComment('${commentId}')">
                        💬 Trả lời
                    </button>
                </div>
                ${comment.replies && Array.isArray(comment.replies) && comment.replies.length > 0 ? this.renderReplies(comment.replies) : ''}
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

    renderReplies(replies) {
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
        if (totalCommentCount) {
            totalCommentCount.textContent = this.totalComments;
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
        // Implement reply functionality
        console.log('Replying to comment:', commentId);
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
        
        const response = await fetch(`https://localhost:7031/api/Review/add`, {
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
        
        const response = await fetch(`https://localhost:7031/api/Property/InterestedProperty/ByRenter/${userId}`, {
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
        
        const response = await fetch(`https://localhost:7031/api/Review/reply/edit`, {
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
        const response = await fetch(`https://localhost:7031/api/Review/post/${propertyId}`, {
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