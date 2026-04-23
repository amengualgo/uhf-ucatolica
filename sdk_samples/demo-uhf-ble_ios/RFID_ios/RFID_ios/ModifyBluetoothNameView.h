//
//  ModifyBluetoothNameView.h
//  RFID_ios
//
//  Created by NLAB on 2020/7/12.
//  Copyright © 2020  . All rights reserved.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface ModifyBluetoothNameView : UIView

/** 点击 save 按钮回调 */
@property (nonatomic, copy) void(^clickSaveBtnBlock)(UIButton *btn,NSString *newName);

@end

NS_ASSUME_NONNULL_END
