//
//  LockChooseeView.h
//  RFID_ios
//
//  Created by hjl on 2018/11/7.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface LockChooseeView : UIView

@property (nonatomic,strong)UIButton *openBtn;
@property (nonatomic,strong)UIButton *lockBtn;
@property (nonatomic,strong)UIButton *longLockBtn;
@property (nonatomic,strong)UIButton *killBtn;
@property (nonatomic,strong)UIButton *accessBtn;
@property (nonatomic,strong)UIButton *epcBtn;
@property (nonatomic,strong)UIButton *tidBtn;
@property (nonatomic,strong)UIButton *userBtn;

@property (nonatomic,copy)void (^returnBlock)(NSString *str);



@end
