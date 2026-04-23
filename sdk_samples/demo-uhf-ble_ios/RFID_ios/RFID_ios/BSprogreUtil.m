//
//  BSprogreUtil.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "BSprogreUtil.h"

@implementation BSprogreUtil

+ (void)showMBProgressWith:(UIView *)superView mode:(MBProgressHUDMode)mode tipString :(NSString *)tip afterDelay:(NSTimeInterval)delay
{
    MBProgressHUD *hud = [[MBProgressHUD alloc] init];
    hud.mode = mode;
    [superView addSubview:hud];
    hud.labelText = tip;
    
    [hud show:YES];
    [hud hide:YES afterDelay:delay];
    [hud removeFromSuperViewOnHide];
}

+ (void)showMBProgressWith:(UIView *)superView andTipString:(NSString *)tip hideTime:(NSInteger)time {
    [BSprogreUtil showMBProgressWith:superView mode:MBProgressHUDModeText tipString:tip afterDelay:time];
}

+ (void)showMBProgressWith:(UIView *)superView andTipString:(NSString *)tip autoHide:(BOOL)hide
{
    if (hide) {
        [BSprogreUtil showMBProgressWith:superView mode:MBProgressHUDModeText tipString:tip afterDelay:1];
    } else {
        [BSprogreUtil showMBProgressWith:superView mode:MBProgressHUDModeIndeterminate tipString:tip afterDelay:NSNotFound];
    }
}

+ (void)showMBProgressWith:(UIView *)superView // 添加的子视图
                    TipStr:(NSString *)tipStr  // 添加的text
             afterHideTime:(NSTimeInterval)afterHideTime
               finishBlock:(void(^)(void))finishBlock
{
    [BSprogreUtil showMBProgressWith:superView TipStr:tipStr MBPMode:MBProgressHUDModeText isDimBackground:NO afterHideTime:afterHideTime finishBlock:finishBlock];
}
+ (void)showMBProgressWith:(UIView *)superView // 添加的子视图
                    TipStr:(NSString *)tipStr  // 添加的text
                   MBPMode:(MBProgressHUDMode)MBPMode // 选择模式(菊花、文字、菊花与文字)
           isDimBackground:(BOOL)isDimBackground
             afterHideTime:(NSTimeInterval)afterHideTime
               finishBlock:(void(^)(void))finishBlock
{
    
    /*
     MBPMode:
     
     MBProgressHUDModeText           纯文本
     MBProgressHUDModeIndeterminate  菊花加文本
     */
    
    MBProgressHUD *hud = [[MBProgressHUD alloc] init];
    hud.labelText = tipStr;
    hud.mode = MBPMode;
    hud.dimBackground = isDimBackground;
    [hud show:YES];
    [superView addSubview:hud];
    
    // 延时调用
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(afterHideTime * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
        
        // 隐藏遮罩并且删除
        [hud hide:YES];
        [hud removeFromSuperViewOnHide];
        
        // 回调block
        finishBlock();
    });
}

@end
