//
//  SettingView.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Masonry/Masonry.h>

@interface SettingView : UIView <UIScrollViewDelegate>

@property (nonatomic,strong)UILabel *workLab;
@property (nonatomic,strong)UIButton *setFrequencyBtn;
@property (nonatomic,strong)UIButton *getFrequencyBtn;
@property (nonatomic,strong)UILabel *usaLab;
@property (nonatomic,strong)UILabel *brazilLab;
@property (nonatomic,strong)UILabel *otherLab;

@property (nonatomic,strong)UIButton *setdianBtn;

@property (nonatomic,strong)UILabel *putLab;
@property (nonatomic,strong)UIButton *setPowerBtn;
@property (nonatomic,strong)UIButton *getPowerBtn;


@property (nonatomic,strong)UIButton *workBtn;
@property (nonatomic,strong)UIButton *hopBtn;
@property (nonatomic,strong)UIButton *putBtn;

@property (nonatomic,strong)UIButton *usaBtn;
@property (nonatomic,strong)UIButton *brazilBtn;
@property (nonatomic,strong)UIButton *otherBtn;

@property (nonatomic,strong)UILabel *userPtrLab;
@property (nonatomic,strong)UIButton *epcBtn;
@property (nonatomic,strong)UIButton *epcTidBtn;
@property (nonatomic,strong)UIButton *epcTidUserBtn;
@property (nonatomic,strong)UIView *containerView;
@property (nonatomic,strong)UITextField *userText;
@property (nonatomic,strong)UITextField *userLengthText;
@property (nonatomic,strong)UIButton *setScanModelBtn;
@property (nonatomic,strong)UIButton *getScanModelBtn;


@property (nonatomic,strong)UIButton *buzzerOpen;
@property (nonatomic,strong)UIButton *buzzerClose;

@property (nonatomic,strong)UITextField *parameterField;
@property (nonatomic,strong)UIButton *setParameterBtn;

@property (nonatomic,strong)UITextField *timeoutField;
@property (nonatomic,strong)UIButton *setTimeoutBtn;

@property (nonatomic,strong)UIButton *rssiYesBtn;
@property (nonatomic,strong)UIButton *rssiNoBtn;

@property (nonatomic,strong)UIButton *codeIdYesBtn;
@property (nonatomic,strong)UIButton *codeIdNoBtn;

@property (nonatomic,copy)void (^workBlock)(void);
@property (nonatomic,copy)void (^hopBlock)(void);
@property (nonatomic,copy)void (^putBlock)(void);

@property (nonatomic,copy)void (^usaBlock)(void);
@property (nonatomic,copy)void (^brazilBlock)(void);
@property (nonatomic,copy)void (^otherBlock)(void);

//@property (nonatomic,copy)void (^epcBlock)(void);
//@property (nonatomic,copy)void (^epcTidBlock)(void);
//@property (nonatomic,copy)void (^epcTidUserBlock)(void);
@property (nonatomic,copy)void (^setScanModelBtnBlock)(void);
@property (nonatomic,copy)void (^getScanModelBtnBlock)(void);

@property (nonatomic,copy)void (^buzzerOpenBlock)(void);
@property (nonatomic,copy)void (^buzzerCloseBlock)(void);


@property (nonatomic,copy)void (^setFrequencyBlock)(void);
@property (nonatomic,copy)void (^getFrequencyBlock)(void);
@property (nonatomic,copy)void (^setHotBlock)(void);
@property (nonatomic,copy)void (^setPowerBlock)(void);
@property (nonatomic,copy)void (^getPowerBlock)(void);


@property (nonatomic,copy)void (^setParamerBtnBlock)(void);

@property (nonatomic,copy)void (^rssiYesBtnBlock)(void);
@property (nonatomic,copy)void (^rssiNoBtnBlock)(void);

@property (nonatomic,copy)void (^codeIdOpenBtnBlock)(void);
@property (nonatomic,copy)void (^codeIdCloseBtnBlock)(void);

@property (nonatomic,copy)void (^setTimeoutBtnBlock)(void);


@property (nonatomic,strong,readonly)NSString *chooseStr;

-(void)chooseSelectWith:(NSString *)chooseStr;

@property (nonatomic, strong) MASConstraint *containerHeightConstraint;
@property (nonatomic, strong) MASConstraint *containerTopConstraint;
@property (nonatomic, strong) MASConstraint *userPtrLabHeightConstraint;

@end
