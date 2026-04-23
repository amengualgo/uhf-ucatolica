//
//  UpgradeView.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UpgradeView : UIView

@property (nonatomic,strong)UIButton *upgradeBtn;
@property (nonatomic,strong)UIButton *readVersonBtn;
@property (nonatomic,strong)UIButton *chooseBtn;
@property (nonatomic,strong)UITextField *fileText;


@property (nonatomic,strong)UIButton *radioBtn;
@property (nonatomic,strong)UIButton *boardBtn;

@property (nonatomic,copy)void (^upgradeBlock)(void);
@property (nonatomic,copy)void (^readVersonBlock)(void);


@end
