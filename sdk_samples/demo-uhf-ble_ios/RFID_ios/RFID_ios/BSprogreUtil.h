//
//  BSprogreUtil.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MBProgressHUD/MBProgressHUD.h>

@interface BSprogreUtil : NSObject

+ (void)showMBProgressWith:(UIView *)superView
              andTipString:(NSString *)tip
                  hideTime:(NSInteger)time;//显示时间，单位秒

+ (void)showMBProgressWith:(UIView *)superView
              andTipString:(NSString *)tip
                  autoHide:(BOOL)hide;//是否自动隐藏

+ (void)showMBProgressWith:(UIView *)superView mode:(MBProgressHUDMode)mode tipString :(NSString *)tip afterDelay:(NSTimeInterval)delay;

+ (void)showMBProgressWith:(UIView *)superView // 添加的子视图
                    TipStr:(NSString *)tipStr  // 添加的text
             afterHideTime:(NSTimeInterval)afterHideTime
               finishBlock:(void(^)(void))finishBlock;

+ (void)showMBProgressWith:(UIView *)superView // 添加的子视图
                    TipStr:(NSString *)tipStr  // 添加的text
                   MBPMode:(MBProgressHUDMode)MBPMode // 选择模式(菊花、文字、菊花与文字)
           isDimBackground:(BOOL)isDimBackground
             afterHideTime:(NSTimeInterval)afterHideTime
               finishBlock:(void(^)(void))finishBlock;
@end
