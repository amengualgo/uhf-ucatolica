//
//  QRCodeScanTypeSelectBtn.h
//  RFID_ios
//
//  Created by   on 2020/10/11.
//  Copyright © 2020  . All rights reserved.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface QRCodeScanTypeSelectBtn : UIButton

/** defalutBtnClickBlock */
@property (nonatomic, copy) void(^clickDefaultBtnBlock)(UIButton *btn);
/** utf8BtnClickBlock */
@property (nonatomic, copy) void(^clickUtf8BtnBlock)(UIButton *btn);
/** gb2312BtnClickBlock */
@property (nonatomic, copy) void(^clickGb2312BtnBlock)(UIButton *btn);

-(instancetype)init;

@end

NS_ASSUME_NONNULL_END
