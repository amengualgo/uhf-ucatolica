//
//  EncryptionView.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface EncryptionView : UIView

@property (nonatomic,strong)UIButton *setmiyaoBtn;
@property (nonatomic,strong)UIButton *getmiyaoBtn;

@property (nonatomic,strong)UIButton *modeBtn;
@property (nonatomic,strong)UITextField *textField1;
@property (nonatomic,strong)UITextField *textField2;
@property (nonatomic,strong)UITextField *textField3;
@property (nonatomic,strong)UITextField *textField4;

@property (nonatomic,copy)void (^setmiBlock)(void);
@property (nonatomic,copy)void (^getmiBlock)(void);
@property (nonatomic,copy)void (^modelBlock)(void);
@property (nonatomic,copy)void (^encryBlock)(void);
@property (nonatomic,copy)void (^dencryBlock)(void);


@end
