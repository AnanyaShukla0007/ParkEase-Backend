using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.DTOs;
using Notification.Application.Interfaces;

namespace Notification.API.Controllers;

[ApiController]
[Route("api/v1/notifications")]
[Produces("application/json")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>Send a notification</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Dispatches a single notification over the requested channel.</remarks>
    [HttpPost]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> Send([FromBody] SendNotificationRequest request)
    {
        var result = await _notificationService.SendAsync(request);

        return Ok(new
        {
            success = true,
            message = "Notification sent successfully.",
            data = result
        });
    }

    /// <summary>Send notifications in bulk</summary>
    /// <remarks>Access: MANAGER, ADMIN. Sends the same notification content to multiple recipients.</remarks>
    [HttpPost("bulk")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> SendBulk([FromBody] SendBulkNotificationsRequest request)
    {
        var result = await _notificationService.SendBulkAsync(request);

        return Ok(new
        {
            success = true,
            message = "Bulk notifications sent successfully.",
            data = result
        });
    }

    /// <summary>Get notifications by recipient</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Returns all notifications for a recipient ordered by latest first.</remarks>
    [HttpGet("recipient/{recipientId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByRecipient(int recipientId)
    {
        return Ok(new
        {
            success = true,
            data = await _notificationService.GetByRecipientAsync(recipientId)
        });
    }

    /// <summary>Get unread notifications by recipient</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Returns only unread notifications for the selected recipient.</remarks>
    [HttpGet("recipient/{recipientId:int}/unread")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUnreadByRecipient(int recipientId)
    {
        return Ok(new
        {
            success = true,
            data = await _notificationService.GetUnreadByRecipientAsync(recipientId)
        });
    }

    /// <summary>Get unread notification count</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Returns the unread badge count for the notification bell.</remarks>
    [HttpGet("recipient/{recipientId:int}/unread-count")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUnreadCount(int recipientId)
    {
        return Ok(new
        {
            success = true,
            data = await _notificationService.GetUnreadCountAsync(recipientId)
        });
    }

    /// <summary>Mark a notification as read</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Marks a single notification as read.</remarks>
    [HttpPut("{notificationId:int}/read")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        var result = await _notificationService.MarkAsReadAsync(notificationId);

        return Ok(new
        {
            success = true,
            message = "Notification marked as read.",
            data = result
        });
    }

    /// <summary>Mark all notifications as read</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Marks all unread notifications as read for the selected recipient.</remarks>
    [HttpPut("recipient/{recipientId:int}/read-all")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> MarkAllRead(int recipientId)
    {
        var affected = await _notificationService.MarkAllReadAsync(recipientId);

        return Ok(new
        {
            success = true,
            message = "All notifications marked as read.",
            data = affected
        });
    }

    /// <summary>Get all notifications</summary>
    /// <remarks>Access: ADMIN. Returns the full notification history across the platform.</remarks>
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        return Ok(new
        {
            success = true,
            data = await _notificationService.GetAllAsync()
        });
    }

    /// <summary>Delete notification</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Deletes a notification by ID.</remarks>
    [HttpDelete("{notificationId:int}")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> Delete(int notificationId)
    {
        await _notificationService.DeleteNotificationAsync(notificationId);

        return Ok(new
        {
            success = true,
            message = "Notification deleted successfully."
        });
    }
}
