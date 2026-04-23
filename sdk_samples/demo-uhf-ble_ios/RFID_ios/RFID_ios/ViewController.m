//
//  ViewController.m
//  RFID_ios
//
//  Created by  on 2018/4/26.
//  Copyright © 2018年 . All rights reserved.
//

#import "ViewController.h"
#import "BSprogreUtil.h"
#import "RFIDBlutoothManager.h"
#import "Masonry.h"
#import "AppDelegate.h"
#import "BLEModel.h"
#import "CustomNaviAlertView.h"
#import <IQKeyboardManager.h>
#import "TopView.h"
#import "ScanView.h"
#import "SettingView.h"
#import "EncryptionView.h"
#import "USERView.h"
#import "ReadMessageView.h"
#import "WriteMessageView.h"
#import "LockView.h"
#import "LockChooseeView.h"

#import "KillView.h"
#import "UpgradeView.h"

#import "ChooseView.h"
#import "QRcodeView.h"

#import "ModifyBluetoothNameView.h"


#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
#define weakSelf(self) __weak typeof(self)weakSelf = self
#define kDefine 5
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]

@interface ViewController ()<PeripheralAddDelegate,FatScaleBluetoothManager,UIScrollViewDelegate>

@property (nonatomic,strong)TopView *topView;
@property (nonatomic,strong)UIScrollView *middleScrollView;

@property (nonatomic,strong)ScanView *scanView;
@property (nonatomic,strong)QRcodeView *codeView;
@property (nonatomic,strong)SettingView *settingview;
@property (nonatomic,strong)EncryptionView *encryView;
@property (nonatomic,strong)USERView *userrView;
@property (nonatomic,strong)ReadMessageView *readView;
@property (nonatomic,strong)WriteMessageView *writeView;
@property (nonatomic,strong)LockView *lockView;
@property (nonatomic,strong)LockChooseeView *lockchooseVieww;

@property (nonatomic,strong)KillView *destoryView;
@property (nonatomic,strong)UpgradeView *upgradeView;
/** modify bluetooth name */
@property (nonatomic, strong) ModifyBluetoothNameView *modifyNameView;

@property (nonatomic ,strong)CustomNaviAlertView *searchAlert;

@property (nonatomic,strong)ChooseView *chooseView;


@property (nonatomic,strong)UIView *linView;

@property (nonatomic,strong)NSMutableArray *dataLinView;
@property (nonatomic,strong)NSMutableArray *dataBottomView;

@property (nonatomic,strong)NSMutableArray *modelArr;  //设置
@property (nonatomic,strong)NSMutableArray *hopArr;  //设置
@property (nonatomic,strong)NSMutableArray *putArr;   //设置

@property (nonatomic,strong)NSMutableArray *encryModelArr;   //加密
@property (nonatomic,strong)NSMutableArray *saveReadArr;   //读数据
@property (nonatomic,strong)NSMutableArray *saveWriteArr;   //写数据

@property (nonatomic,strong)NSMutableArray *updateArr;   //写数据

@property (nonatomic,strong)NSMutableArray *rcodeArr;   //扫描到的二维码数据


@property (nonatomic,assign)NSInteger modelIndex;
@property (nonatomic,assign)NSInteger hopIndex;
@property (nonatomic,assign)NSInteger putIndex;

@property (nonatomic,assign)NSInteger updateIndex;//写数据的计算

@property (nonatomic,assign)NSInteger encryModelIndex;
@property (nonatomic,assign)NSInteger saveReadIndex;
@property (nonatomic,assign)NSInteger saveWriteIndex;


@property (nonatomic,assign)NSInteger chooseIndex;


@property (nonatomic,assign)BOOL isConnect;

@end

@implementation ViewController


- (CustomNaviAlertView *) searchAlert {
    if (_searchAlert == nil) {
//        _searchAlert = [[CustomNaviAlertView alloc]initWithFrame:CGRectMake(kDefine, ([UIScreen mainScreen].bounds.size.height-64)/4-80, [UIScreen mainScreen].bounds.size.width-kDefine*2, ([UIScreen mainScreen].bounds.size.height-64)/2)];
//        _searchAlert = [[CustomNaviAlertView alloc]initWithFrame:CGRectMake(15, AdaptH(160), [UIScreen mainScreen].bounds.size.width-30, AdaptH(380))];
        _searchAlert = [[CustomNaviAlertView alloc]initWithFrame:self.view.bounds];
        weakSelf(self);
        
        _searchAlert.cancelBlock = ^() {
            [[RFIDBlutoothManager shareManager] startBleScan];
            [weakSelf.searchAlert.dataSource removeAllObjects];
            [weakSelf.searchAlert.myTableView reloadData];
            
        };
        _searchAlert.removeBlock=^() {
            [weakSelf.searchAlert removeFromSuperview];
            weakSelf.searchAlert=nil;
            [[RFIDBlutoothManager shareManager] closeBleAndDisconnect];
            
        };
        
    }
    return _searchAlert;
}


-(void)getUpdateData {
    NSString *path = [[NSBundle mainBundle] pathForResource:@"Reader_v2_0_4" ofType:@"bin"];
    NSData *reader = [NSData dataWithContentsOfFile:path];
    
    NSString *dataStr=[BluetoothUtil stringWithHexBytes2:reader];
    NSLog(@"dataStr=======%@",dataStr);
    NSLog(@"dataStr.length===%lu",(unsigned long)dataStr.length);
    
    _updateArr=[[NSMutableArray alloc]init];
    
    NSString *str;
    for (NSInteger i=0; i<dataStr.length; i++) {
        
        str=[NSString stringWithFormat:@"%@%@",str,[dataStr substringWithRange:NSMakeRange(i,1)]];
        if (str.length==128) {
            [_updateArr addObject:str];
            str=@"";
        }
        if (i==dataStr.length-1) {
            NSLog(@"str.length====%lu",(unsigned long)str.length);//46  82
            NSInteger countt = str.length;
            for (NSInteger j=0; j<128-countt; j++) {
                str=[NSString stringWithFormat:@"%@%@",str,@"0"];
            }
            [_updateArr addObject:str];
        }
    }
}

#pragma mark - 页面初始化 View init
- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    
    Byte rssi[2];
    rssi[0]=0xFD;
    rssi[1]=0x6F;
    int irssi = (rssi[0] & 0xFF) | (rssi[1] & 0x8000);
    float dBm = (65535 - irssi) / 10.0;
    NSLog(@"dBm=====%lf",dBm);
    
    self.dataBottomView=[[NSMutableArray alloc]init];
    self.isConnect=NO;
    
    [self initModelData];
    //设置代理
    [[RFIDBlutoothManager shareManager] setFatScaleBluetoothDelegate:self];
    
    [self initTopView];
    [self initScrollView];
    [self initScanView];
    [self initQrcodeView];
    [self initSettingView];
    // [self initEncryview];
    // [self initUserView];
    [self initReadView];
    [self initWriteView];
    [self initLockView];
    [self initKillView];
    // [self initUpgradeView];
    // [self initModifyBluetoothNameView];
    // [self ini tLockChooseView];
    [self initRigntBtn];
    
    self.chooseView=[ChooseView new];
    self.chooseView.frame=CGRectMake(0, 0, [UIScreen mainScreen].bounds.size.width, [UIScreen mainScreen].bounds.size.height);
    [[AppDelegate sharedDelegate].window addSubview:self.chooseView];
    [[AppDelegate sharedDelegate].window bringSubviewToFront:self.chooseView];
    self.chooseView.hidden=YES;
    
    self.rcodeArr=[[NSMutableArray alloc]init];
    
    //NSString *tagStr = @"C8 8C 00 25 E1 00 00 02 0C E2 00 00 17 01 0B 00 49 17 50 61 77 0C E2 00 00 17 01 0B 00 25 17 50 61 47 9A 0D 0A";
    //NSString *tagStr = [NSString stringWithFormat:@"%@",@"C88C0025E10000020CE2000017010B0049175061770CE2000017010B0025175061479A0D0A"];
    
    [self getUpdateData];
}

-(void) initModelData {
    // initModelArr
    self.modelArr=[[NSMutableArray alloc]init];
    [self.modelArr addObject:@"China Standard1(840~845MHZ)"];
    [self.modelArr addObject:@"China Standard2(920~925MHZ)"];
    [self.modelArr addObject:@"Europe Standard(865~868MHZ)"];
    [self.modelArr addObject:@"America Standard(902~928MHZ)"];
    [self.modelArr addObject:@"Korea Standard(917~923MHZ)"];
    [self.modelArr addObject:@"Japan Standard(952~953MHZ)"];
    
    //initHopArr
    self.hopArr=[[NSMutableArray alloc]init];
    for (NSInteger i=0; i<50; i++) {
        [self.hopArr addObject:[NSString stringWithFormat:@"%.2lf",902.75+i*0.5]];
    }
    
    //initPutArr.
    self.putArr=[[NSMutableArray alloc]init];
    for (NSInteger i=0; i<26; i++) {
        [self.putArr addObject:[NSString stringWithFormat:@"%ld", 5 + i]];
    }
    
    //initencryModelArr
    self.encryModelArr=[[NSMutableArray alloc]init];
    [self.encryModelArr addObject:@"ECB"];
    [self.encryModelArr addObject:@"CBC"];
    [self.encryModelArr addObject:@"OFB"];
    [self.encryModelArr addObject:@"CFB"];
    
    //initsaveReadArr
    self.saveReadArr=[[NSMutableArray alloc]init];
    [self.saveReadArr addObject:@"RESERVED"];
    [self.saveReadArr addObject:@"EPC"];
    [self.saveReadArr addObject:@"TID"];
    [self.saveReadArr addObject:@"USER"];
    
    //initsaveWriteArr
    self.saveWriteArr=[[NSMutableArray alloc]init];
    [self.saveWriteArr addObject:@"RESERVED"];
    [self.saveWriteArr addObject:@"EPC"];
    [self.saveWriteArr addObject:@"TID"];
    [self.saveWriteArr addObject:@"USER"];
    
    self.saveReadIndex = 1;
}

-(void)initLockChooseView {
    weakSelf(self);
    self.lockchooseVieww=[LockChooseeView new];
    [self.view addSubview:self.lockchooseVieww];
    [self.lockchooseVieww mas_makeConstraints:^(MASConstraintMaker *make) {
        make.edges.equalTo(self.view);
    }];
    self.lockchooseVieww.returnBlock = ^(NSString *str) {
        weakSelf.lockView.lockWordField.text=str;
    };
    [self.view bringSubviewToFront:self.lockchooseVieww];
}

-(void)initRigntBtn
{
    UIButton *button=[UIButton buttonWithType:UIButtonTypeCustom];
    [button setTitle:@"..." forState:UIControlStateNormal];
    button.titleLabel.font=[UIFont boldSystemFontOfSize:28];
    [button setTitleColor:[UIColor whiteColor] forState:UIControlStateNormal];
    [button addTarget:self action:@selector(rightbtnn) forControlEvents:UIControlEventTouchUpInside];
    [self.view addSubview:button];
    [button mas_makeConstraints:^(MASConstraintMaker *make) {
        make.right.equalTo(self.view).offset(-5);
        make.top.equalTo(self.view).offset(15);
        make.width.mas_equalTo(80);
        make.height.mas_equalTo(35);
    }];
}

-(void)rightbtnn {
    UIAlertController *alert=[UIAlertController alertControllerWithTitle:nil message:nil preferredStyle:UIAlertControllerStyleActionSheet];
    UIAlertAction *action1=[UIAlertAction actionWithTitle:@"Access to electricity" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        if([RFIDBlutoothManager shareManager].isgetLab) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"Please stop scan first" autoHide:YES];
            return;
        }
        //   [self initLockChooseView];
        [[RFIDBlutoothManager shareManager] getBatteryLevel];
    }];
    UIAlertAction *action2=[UIAlertAction actionWithTitle:@"Get version" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        if([RFIDBlutoothManager shareManager].isgetLab) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"Please stop scan first" autoHide:YES];
            return;
        }
        [[RFIDBlutoothManager shareManager] getFirmwareVersion];//固件版本号
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(2 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
            [[RFIDBlutoothManager shareManager] getHardwareVersion];//硬件版本号
        });
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(4 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
            [[RFIDBlutoothManager shareManager] getReaderMainboardVersion];//主板版本号
        });
    }];
    UIAlertAction *action3=[UIAlertAction actionWithTitle:@"The module temperature" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        if([RFIDBlutoothManager shareManager].isgetLab) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"Please stop scan first" autoHide:YES];
            return;
        }
        [[RFIDBlutoothManager shareManager] getServiceTemperature];//模块温度
    }];
    
/*
    UIAlertAction *action4=[UIAlertAction actionWithTitle:@"升级" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        [[RFIDBlutoothManager shareManager] enterUpgradeMode];//进入升级模式
        sleep(2);
        [[RFIDBlutoothManager shareManager] enterUpgradeAcceptData];//进入升级接收数据
    }];
    UIAlertAction *action5=[UIAlertAction actionWithTitle:@"退出升级模式" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        [[RFIDBlutoothManager shareManager] exitUpgradeMode];//退出升级模式
        
    }];
*/
    UIAlertAction *action6=[UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleCancel handler:^(UIAlertAction * _Nonnull action) {
    }];
    [alert addAction:action1];
    [alert addAction:action2];
    [alert addAction:action3];
    //[alert addAction:action4];
    //[alert addAction:action5];
    [alert addAction:action6];
    
    UIPopoverPresentationController *popover = alert.popoverPresentationController;
    if (popover) {
        popover.sourceView = self.view;
        popover.sourceRect = CGRectMake(self.view.frame.size.width - 100, 0, 100, 100);
        popover.permittedArrowDirections = UIPopoverArrowDirectionAny;
    }
    
    
    [self presentViewController:alert animated:YES completion:nil];
}

-(void)initTopView {
    self.topView=[TopView new];
    [self.view addSubview:self.topView];
    [self.topView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.top.equalTo(self.view);
        make.height.mas_equalTo(AdaptH(170));
    }];
    weakSelf(self);
//    self.topView.searchBlock = ^{
//        if([RFIDBlutoothManager shareManager].isgetLab) {
//            [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"Please stop scan first" autoHide:YES];
//            return;
//        }
//        [[RFIDBlutoothManager shareManager] startBleScan];
//        [weakSelf.searchAlert.dataSource removeAllObjects];
//        [weakSelf.searchAlert.myTableView reloadData];
//        [weakSelf.view addSubview:weakSelf.searchAlert];
//    };
    self.topView.connectBlock = ^{
        if([RFIDBlutoothManager shareManager].isgetLab) {
            [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"Please stop scan first" autoHide:YES];
            return;
        }
        if (weakSelf.isConnect) {
            [[RFIDBlutoothManager shareManager] cancelConnectBLE];
        } else {
            [[RFIDBlutoothManager shareManager] startBleScan] ;
            [weakSelf.searchAlert.dataSource removeAllObjects];
            [weakSelf.searchAlert.myTableView reloadData];
            [weakSelf.view addSubview:weakSelf.searchAlert];
        }
    };
}

-(void)initScrollView {
    self.dataLinView=[[NSMutableArray alloc]init];
    
    _middleScrollView=[[UIScrollView alloc]init];
    _middleScrollView.delegate=self;
    _middleScrollView.backgroundColor = [UIColor clearColor];
    _middleScrollView.showsVerticalScrollIndicator = NO;
    _middleScrollView.showsHorizontalScrollIndicator = NO;
    // NSArray *aa=@[@"Inventory",@"Barcode Scan",@"Settings",@"加密",@"USER区加密",@"读数据",@"写数据",@"锁",@"销毁"];
    NSArray *aa=@[@"Scan",@"Barcode Scan",@"Settings",@"Read",@"Write",@"Lock",@"Kill",/*@"upgrade",*//*@"Modify Bluetooth Name"*/];
    //  NSArray *aa=@[@"盘点标签",@"二维码扫描",@"设置",@"读数据",@"写数据",@"锁",@"销毁",@"升级"];
    [self.view addSubview:_middleScrollView];
    [_middleScrollView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.equalTo(self.view);
        make.top.equalTo(self.topView.connectBtn.mas_bottom).offset(6);
        make.height.mas_equalTo(AdaptH(50));
    }];
    _middleScrollView.contentSize=CGSizeMake(AdaptH(150) * aa.count, AdaptH(50));
    for (NSInteger i=0; i<aa.count; i++) {
        UIButton *button=[UIButton buttonWithType:UIButtonTypeCustom];
        [_middleScrollView addSubview:button];
        [button setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        button.tag=i;
        button.titleLabel.font=[UIFont systemFontOfSize:15];
        [button setTitle:aa[i] forState:UIControlStateNormal];
        [button addTarget:self action:@selector(chooseBtn:) forControlEvents:UIControlEventTouchUpInside];
        button.frame=CGRectMake(i*AdaptH(150), 0, AdaptH(150), AdaptH(50));
        
        UIView *linView=[UIView new];
        [_middleScrollView addSubview:linView];
        linView.backgroundColor=RGB(230, 230, 230, 1);
        linView.frame=CGRectMake(i*AdaptH(150), AdaptH(5), 1, AdaptH(35));
        
        UIView *linView1=[UIView new];
        [_middleScrollView addSubview:linView1];
        linView1.tag=i;
        linView1.backgroundColor=RGB(200, 200, 200, 1);;
        linView1.frame=CGRectMake(i*AdaptH(150), AdaptH(47), AdaptH(150), 2);
        [self.dataLinView addObject:linView1];
        if (i==0) {
            linView1.hidden=NO;
        } else {
            linView1.hidden=YES;
        }
        
    }
}


-(void)initScanView {
    weakSelf(self);
    self.scanView=[ScanView new];
    [self.view addSubview:self.scanView];
    [self.scanView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(self->_middleScrollView.mas_bottom);
    }];
    self.scanView.dataSource=[[NSMutableArray alloc]init];
    self.scanView.tag=0;
    self.scanView.hidden=NO;
    [self.dataBottomView addObject:self.scanView];
    [self.scanView.tableView reloadData];
    
    self.scanView.setFilterBlock = ^{
        [[RFIDBlutoothManager shareManager] setFilterWithBank:weakSelf.scanView.filterBank Ptr:weakSelf.scanView.ptrField.text Len:weakSelf.scanView.lenField.text Data:weakSelf.scanView.dataField.text];
    };
    self.scanView.singleBlock = ^{
        [[RFIDBlutoothManager shareManager] singleInventory];
    };
    self.scanView.beginBlock = ^{
        [[RFIDBlutoothManager shareManager] startInventory];
    };
    self.scanView.stopBlock = ^{
        //停止扫描标签
        [[RFIDBlutoothManager shareManager] stopInventory];
    };
    
    self.scanView.cleanBlock = ^{
        //清空数据
        weakSelf.scanView.dataSource = [NSMutableArray array];
        weakSelf.scanView.allCount = 0;
        [weakSelf.scanView.tableView reloadData];
        weakSelf.scanView.allCountLab.text = @"0";
        weakSelf.scanView.countLab.text = @"0";
    };
}

-(void)initQrcodeView {
    self.codeView=[QRcodeView new];
    [self.view addSubview:self.codeView];
    [self.codeView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(self->_middleScrollView.mas_bottom);
    }];
    self.codeView.dataSource=[NSMutableArray array];
    self.codeView.tag=1;
    [self.dataBottomView addObject:self.codeView];
    self.codeView.hidden=YES;
    [self.codeView.tableView reloadData];
    weakSelf(self);
    self.codeView.scanBlock = ^{
//        [RFIDBlutoothManager shareManager].isCodeLab = YES;
        [[RFIDBlutoothManager shareManager] start2DScan];
    };
    self.codeView.cleanBlock = ^{
        //清空数据
        weakSelf.codeView.dataSource = [NSMutableArray array];
        weakSelf.rcodeArr=[[NSMutableArray alloc]init];
        [weakSelf.codeView.tableView reloadData];
    };
    self.codeView.continuousScanBlock = ^{
        [[RFIDBlutoothManager shareManager] startContinuity2DScan];
    };
    self.codeView.continuousStopBlock = ^{
        [[RFIDBlutoothManager shareManager] stopContinuity2DScan];
    };
}

-(void)initSettingView {
    self.settingview=[SettingView new];
    [self.view addSubview:self.settingview];
    [self.settingview mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(self->_middleScrollView.mas_bottom);
    }];
    self.settingview.hidden=YES;
    self.settingview.tag=2;
    [self.dataBottomView addObject:self.settingview];
    [self.settingview.workBtn setTitle:self.modelArr[0] forState:UIControlStateNormal];
    [self.settingview.hopBtn setTitle:self.hopArr[0] forState:UIControlStateNormal];
    [self.settingview.putBtn setTitle:self.putArr[0] forState:UIControlStateNormal];
    weakSelf(self);
    self.settingview.workBlock = ^{
        //选工作模式
        [[AppDelegate sharedDelegate].window bringSubviewToFront:weakSelf.chooseView];
        weakSelf.chooseView.hidden=NO;
        weakSelf.chooseView.dataSource=[NSMutableArray arrayWithArray:weakSelf.modelArr];
        [weakSelf.chooseView.tableView reloadData];
        weakSelf.chooseView.removeBlock = ^{
            weakSelf.chooseView.hidden=YES;
            // [weakSelf.chooseView removeFromSuperview];
        };
        weakSelf.chooseView.returnBlock = ^(NSInteger index) {
            //  [weakSelf.chooseView removeFromSuperview];
            weakSelf.modelIndex=index;
            weakSelf.chooseView.hidden=YES;
            [weakSelf.settingview.workBtn setTitle:weakSelf.modelArr[index] forState:UIControlStateNormal];
        };
    };
    self.settingview.setFrequencyBlock = ^{
        //设置频率
        [[RFIDBlutoothManager shareManager] setRegionWithsaveStr:@"1" regionStr:[NSString stringWithFormat:@"%ld",(long)weakSelf.modelIndex]];
        
    };
    self.settingview.getFrequencyBlock = ^{
        //读取频率
        [[RFIDBlutoothManager shareManager] getRegion];
    };
    self.settingview.hopBlock = ^{
        //选择hop
        [[AppDelegate sharedDelegate].window bringSubviewToFront:weakSelf.chooseView];
        weakSelf.chooseView.hidden=NO;
        weakSelf.chooseView.dataSource=[NSMutableArray arrayWithArray:weakSelf.hopArr];
        [weakSelf.chooseView.tableView reloadData];
        weakSelf.chooseView.removeBlock = ^{
            weakSelf.chooseView.hidden=YES;
            // [weakSelf.chooseView removeFromSuperview];
        };
        weakSelf.chooseView.returnBlock = ^(NSInteger index) {
            //  [weakSelf.chooseView removeFromSuperview];
            weakSelf.hopIndex=index;
            weakSelf.chooseView.hidden=YES;
            [weakSelf.settingview.hopBtn setTitle:weakSelf.hopArr[index] forState:UIControlStateNormal];
        };
    };
    self.settingview.setHotBlock = ^{
        //设置频点
        //  [[RFIDBlutoothManager shareManager] getBatteryLevel];
        [[RFIDBlutoothManager shareManager] detailChancelSettingWithstring:weakSelf.hopArr[weakSelf.hopIndex]];
    };
    self.settingview.putBlock = ^{
        //输出功率
        [[AppDelegate sharedDelegate].window bringSubviewToFront:weakSelf.chooseView];
        weakSelf.chooseView.hidden=NO;
        weakSelf.chooseView.dataSource=[NSMutableArray arrayWithArray:weakSelf.putArr];
        [weakSelf.chooseView.tableView reloadData];
        weakSelf.chooseView.removeBlock = ^{
            weakSelf.chooseView.hidden=YES;
            // [weakSelf.chooseView removeFromSuperview];
        };
        weakSelf.chooseView.returnBlock = ^(NSInteger index) {
            //  [weakSelf.chooseView removeFromSuperview];
            weakSelf.putIndex=index;
            weakSelf.chooseView.hidden=YES;
            [weakSelf.settingview.putBtn setTitle:weakSelf.putArr[index] forState:UIControlStateNormal];
        };
    };
    self.settingview.setPowerBlock = ^{
        //设置功率
        [[RFIDBlutoothManager shareManager] setLaunchPowerWithstatus:@"1" antenna:@"1" readStr:weakSelf.putArr[weakSelf.putIndex] writeStr:weakSelf.putArr[weakSelf.putIndex]];
    };
    self.settingview.getPowerBlock = ^{
        //读取功率
        [[RFIDBlutoothManager shareManager] getLaunchPower];
    };
    self.settingview.usaBlock = ^{
        //选择USA
        [weakSelf.hopArr removeAllObjects];
        for (NSInteger i=0; i<50; i++) {
            [weakSelf.hopArr addObject:[NSString stringWithFormat:@"%.2lf",902.75+i*0.5]];
        }
        [weakSelf.settingview.hopBtn setTitle:weakSelf.hopArr[0] forState:UIControlStateNormal];
    };
    self.settingview.brazilBlock = ^{
        //选择brazil
        [weakSelf.hopArr removeAllObjects];
        for (NSInteger i=0; i<50; i++) {
            [weakSelf.hopArr addObject:[NSString stringWithFormat:@"%.1lf",902.5+i*0.5]];
        }
        [weakSelf.settingview.hopBtn setTitle:weakSelf.hopArr[0] forState:UIControlStateNormal];
        
    };
    self.settingview.otherBlock = ^{
        //选择其他
        [weakSelf.hopArr removeAllObjects];
        for (NSInteger i=0; i<50; i++) {
            [weakSelf.hopArr addObject:[NSString stringWithFormat:@"%.3lf",865.700+i*0.6]];
        }
        [weakSelf.settingview.hopBtn setTitle:weakSelf.hopArr[0] forState:UIControlStateNormal];
    };
    
    self.settingview.setScanModelBtnBlock = ^{
        weakSelf.scanView.cleanBlock();
        [[RFIDBlutoothManager shareManager] setEpcTidUserWithAddressStr:weakSelf.settingview.userText.text length:weakSelf.settingview.userLengthText.text epcStr:weakSelf.settingview.chooseStr];
    };
    self.settingview.getScanModelBtnBlock = ^{
        [[RFIDBlutoothManager shareManager] getEpcTidUser];
    };
    
    self.settingview.buzzerOpenBlock = ^{
        //开启蜂鸣器
        [[RFIDBlutoothManager shareManager] setOpenBuzzer];
    };
    self.settingview.buzzerCloseBlock = ^{
        //关闭蜂鸣器
        [[RFIDBlutoothManager shareManager] setCloseBuzzer];
    };
    
    self.settingview.rssiYesBtnBlock = ^{
        [RFIDBlutoothManager shareManager].isSupportRssi = YES;
        [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"success" autoHide:YES];
    };
    self.settingview.rssiNoBtnBlock = ^{
        [RFIDBlutoothManager shareManager].isSupportRssi = NO;
        [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"success" autoHide:YES];
    };
    
    self.settingview.setParamerBtnBlock = ^{
        NSString *parameter = weakSelf.settingview.parameterField.text;
        if ([parameter isEqual:@""] || ![AppHelper isHexString:parameter]) {
            [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"parameter error" autoHide:YES];
            return;
        }
        BOOL res = [[RFIDBlutoothManager shareManager] setBarcodeParmameter:parameter];
        [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:res ? @"success":@"fail" autoHide:YES];
    };
    self.settingview.codeIdOpenBtnBlock = ^{
        BOOL res = [[RFIDBlutoothManager shareManager] setBarcodeCodeId:YES];
        [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:res ? @"success":@"fail" autoHide:YES];
    };
    self.settingview.codeIdCloseBtnBlock = ^{
        BOOL res = [[RFIDBlutoothManager shareManager] setBarcodeCodeId:NO];
        [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:res ? @"success":@"fail" autoHide:YES];
    };
    self.settingview.setTimeoutBtnBlock = ^{
        NSString *timeout = weakSelf.settingview.timeoutField.text;
        float timeoutValue = timeout.floatValue;
        if (timeoutValue == 0) {
            [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"parameter error" autoHide:YES];
            return;
        }
        if(timeoutValue < 0.5 || timeoutValue > 9.9) {
            [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"Parameter out of range" autoHide:YES];
            return;
        }
        BOOL res = [[RFIDBlutoothManager shareManager] setBarcodeTimeout:timeout];
        [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:res ? @"success":@"fail" autoHide:YES];
    };
}

-(void)initEncryview {
    weakSelf(self);
    self.encryView=[EncryptionView new];
    [self.view addSubview:self.encryView];
    [self.encryView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(self->_middleScrollView.mas_bottom);
    }];
    self.encryView.hidden=YES;
    self.encryView.tag=3;
    [self.dataBottomView addObject:self.encryView];
    self.encryView.setmiBlock = ^{
        //设置密钥
        if (weakSelf.encryView.textField1.text.length!=32) {
            [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"Please enter the correct hex key" autoHide:YES];
            return ;
        }
        if (weakSelf.encryView.textField2.text.length!=32) {
            [BSprogreUtil showMBProgressWith:weakSelf.view andTipString:@"Please enter the correct initial hex value" autoHide:YES];
            return ;
        }
        [RFIDBlutoothManager shareManager].typeStr=@"1";
        [[RFIDBlutoothManager shareManager] setSM4PassWordWithmodel:[NSString stringWithFormat:@"%ld",(long)weakSelf.encryModelIndex] password:weakSelf.encryView.textField1.text originPass:weakSelf.encryView.textField2.text];
    };
    self.encryView.getmiBlock = ^{
        //获取密钥
        [RFIDBlutoothManager shareManager].typeStr=@"2";
        [[RFIDBlutoothManager shareManager] getSM4PassWord];
    };
    self.encryView.modelBlock = ^{
        //选择model
        [[AppDelegate sharedDelegate].window bringSubviewToFront:weakSelf.chooseView];
        weakSelf.chooseView.hidden=NO;
        weakSelf.chooseView.dataSource=[NSMutableArray arrayWithArray:weakSelf.encryModelArr];
        [weakSelf.chooseView.tableView reloadData];
        weakSelf.chooseView.removeBlock = ^{
            weakSelf.chooseView.hidden=YES;
            // [weakSelf.chooseView removeFromSuperview];
        };
        weakSelf.chooseView.returnBlock = ^(NSInteger index) {
            //  [weakSelf.chooseView removeFromSuperview];
            weakSelf.encryModelIndex=index;
            weakSelf.chooseView.hidden=YES;
            [weakSelf.encryView.modeBtn setTitle:weakSelf.encryModelArr[index] forState:UIControlStateNormal];
        };
    };
    self.encryView.encryBlock = ^{
        //加密    //数据要是16字节的倍数
        [RFIDBlutoothManager shareManager].typeStr=@"3";
        [[RFIDBlutoothManager shareManager] encryptionPassWordwithmessage:weakSelf.encryView.textField3.text];
    };
    self.encryView.dencryBlock = ^{
        //解密
        [RFIDBlutoothManager shareManager].typeStr=@"4";
        [[RFIDBlutoothManager shareManager] decryptPassWordwithmessage:weakSelf.encryView.textField3.text];
    };
}

-(void)initUserView {
    weakSelf(self);
    self.userrView=[USERView new];
    [self.view addSubview:self.userrView];
    [self.userrView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(_middleScrollView.mas_bottom);
    }];
    self.userrView.hidden=YES;
    self.userrView.tag=4;
    [self.dataBottomView addObject:self.userrView];
    self.userrView.readBlock = ^{
        //读
        [RFIDBlutoothManager shareManager].typeStr=@"6";
        [[RFIDBlutoothManager shareManager] decryptUSERWithaddress:weakSelf.userrView.addressField.text lengthStr:weakSelf.userrView.lengthField.text];//USER解密
    };
    self.userrView.writeBlock = ^{
        //写
        [RFIDBlutoothManager shareManager].typeStr=@"5";
        [[RFIDBlutoothManager shareManager] encryptionUSERWithaddress:weakSelf.userrView.addressField.text lengthStr:weakSelf.userrView.lengthField.text dataStr:weakSelf.userrView.writeField.text];//USER加密
    };
}

-(void)initReadView {
    weakSelf(self);
    self.readView=[ReadMessageView new];
    [self.view addSubview:self.readView];
    [self.readView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(self.middleScrollView.mas_bottom);
    }];
    self.readView.hidden=YES;
    self.readView.tag=5;
    [self.dataBottomView addObject:self.readView];
    [self.readView.saveBtn setTitle:self.saveReadArr[1] forState:UIControlStateNormal];
    self.readView.saveBlock = ^{
        //存储区
        [[AppDelegate sharedDelegate].window bringSubviewToFront:weakSelf.chooseView];
        weakSelf.chooseView.hidden=NO;
        weakSelf.chooseView.dataSource=[NSMutableArray arrayWithArray:weakSelf.saveReadArr];
        [weakSelf.chooseView.tableView reloadData];
        weakSelf.chooseView.removeBlock = ^{
            weakSelf.chooseView.hidden=YES;
            // [weakSelf.chooseView removeFromSuperview];
        };
        weakSelf.chooseView.returnBlock = ^(NSInteger index) {
            //  [weakSelf.chooseView removeFromSuperview];
            weakSelf.saveReadIndex=index;
            weakSelf.chooseView.hidden=YES;
            [weakSelf.readView.saveBtn setTitle:weakSelf.saveReadArr[index] forState:UIControlStateNormal];
            if (index==0) {
                weakSelf.readView.addressField1.text =@"0";
                weakSelf.readView.lengthField1.text=@"4";
            } else if (index==1) {
                weakSelf.readView.addressField1.text =@"2";
                weakSelf.readView.lengthField1.text=@"6";
            } else if (index==2) {
                weakSelf.readView.addressField1.text =@"0";
                weakSelf.readView.lengthField1.text=@"6";
            } else if (index==3) {
                weakSelf.readView.addressField1.text =@"0";
                weakSelf.readView.lengthField1.text=@"6";
            }
        };
    };
    self.readView.readMessageBlock = ^{
        //读取数据
        [[RFIDBlutoothManager shareManager] readLabelMessageWithPassword:weakSelf.readView.passWordField1.text MMBstr:weakSelf.readView.typeStr MSAstr:weakSelf.readView.addressField.text MDLstr:weakSelf.readView.lengthField.text MDdata:weakSelf.readView.dataField.text MBstr:[NSString stringWithFormat:@"%ld",(long)weakSelf.saveReadIndex] SAstr:weakSelf.readView.addressField1.text DLstr:weakSelf.readView.lengthField1.text isfilter:weakSelf.readView.isFilter];
    };
}

-(void)initWriteView {
    weakSelf(self);
    self.writeView=[WriteMessageView new];
    [self.view addSubview:self.writeView];
    [self.writeView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(_middleScrollView.mas_bottom);
    }];
    self.writeView.hidden=YES;
    self.writeView.tag=6;
    [self.dataBottomView addObject:self.writeView];
    [self.writeView.saveBtn setTitle:self.saveWriteArr[0] forState:UIControlStateNormal];
    self.writeView.saveBlock = ^{
        //存储区
        [[AppDelegate sharedDelegate].window bringSubviewToFront:weakSelf.chooseView];
        weakSelf.chooseView.hidden=NO;
        weakSelf.chooseView.dataSource=[NSMutableArray arrayWithArray:weakSelf.saveWriteArr];
        [weakSelf.chooseView.tableView reloadData];
        weakSelf.chooseView.removeBlock = ^{
            weakSelf.chooseView.hidden=YES;
            // [weakSelf.chooseView removeFromSuperview];
        };
        weakSelf.chooseView.returnBlock = ^(NSInteger index) {
            //  [weakSelf.chooseView removeFromSuperview];
            weakSelf.saveWriteIndex=index;
            NSLog(@"index==%ld",(long)index);
            weakSelf.chooseView.hidden=YES;
            [weakSelf.writeView.saveBtn setTitle:weakSelf.saveWriteArr[index] forState:UIControlStateNormal];
            
            if (index==0) {
                weakSelf.writeView.addressField1.text =@"0";
                weakSelf.writeView.lengthField1.text=@"4";
            }
            else if (index==1)
            {
                weakSelf.writeView.addressField1.text =@"2";
                weakSelf.writeView.lengthField1.text=@"6";
            }
            else if (index==2)
            {
                weakSelf.writeView.addressField1.text =@"0";
                weakSelf.writeView.lengthField1.text=@"6";
            }
            else if (index==3)
            {
                weakSelf.writeView.addressField1.text =@"0";
                weakSelf.writeView.lengthField1.text=@"6";
            }
            
        };
    };
    self.writeView.writeMessageBlock = ^{
        //NSLog(@"saveWriteIndex====%@",[NSString stringWithFormat:@"%ld",(long)weakSelf.saveWriteIndex]);
        //写入数据
        [[RFIDBlutoothManager shareManager] writeLabelMessageWithPassword:weakSelf.writeView.passWordField1.text MMBstr:weakSelf.writeView.typeStr MSAstr:weakSelf.writeView.addressField.text MDLstr:weakSelf.writeView.lengthField.text MDdata:weakSelf.writeView.dataField.text MBstr:[NSString stringWithFormat:@"%ld",(long)weakSelf.saveWriteIndex] SAstr:weakSelf.writeView.addressField1.text DLstr:weakSelf.writeView.lengthField1.text writeData:weakSelf.writeView.dataField1.text isfilter:weakSelf.writeView.isFilter];
    };
}
-(void)initLockView
{
    weakSelf(self);
    self.lockView=[LockView new];
    [self.view addSubview:self.lockView];
    [self.lockView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(_middleScrollView.mas_bottom);
    }];
    self.lockView.hidden=YES;
    self.lockView.tag=7;
    [self.dataBottomView addObject:self.lockView];
    self.lockView.lockBlock = ^{
        //锁
        [[RFIDBlutoothManager shareManager] lockLabelWithPassword:weakSelf.lockView.passWordField.text MMBstr:weakSelf.lockView.typeStr MSAstr:weakSelf.lockView.addressField.text MDLstr:weakSelf.lockView.lengthField.text MDdata:weakSelf.lockView.dataField.text ldStr:weakSelf.lockView.lockWordField.text isfilter:weakSelf.lockView.isFilter];
    };
    self.lockView.showlockBlock = ^{
        NSLog(@"lockBlock");
        [weakSelf initLockChooseView];
    };
}
-(void)initKillView
{
    weakSelf(self);
    self.destoryView=[KillView new];
    [self.view addSubview:self.destoryView];
    [self.destoryView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(_middleScrollView.mas_bottom);
    }];
    self.destoryView.hidden=YES;
    self.destoryView.tag=8;
    [self.dataBottomView addObject:self.destoryView];
    self.destoryView.destoryBlock = ^{
        //销毁
        [[RFIDBlutoothManager shareManager] killLabelWithPassword:weakSelf.destoryView.passWordField.text MMBstr:weakSelf.destoryView.typeStr MSAstr:weakSelf.destoryView.addressField.text MDLstr:weakSelf.destoryView.lengthField.text MDdata:weakSelf.destoryView.dataField.text isfilter:weakSelf.destoryView.isFilter];
    };
}

-(void)initUpgradeView
{
    self.upgradeView=[UpgradeView new];
    [self.view addSubview:self.upgradeView];
    [self.upgradeView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(_middleScrollView.mas_bottom);
    }];
    self.upgradeView.hidden=YES;
    self.upgradeView.tag=9;
    [self.dataBottomView addObject:self.upgradeView];
    self.upgradeView.upgradeBlock = ^{
        //进入升级模式
        [[RFIDBlutoothManager shareManager] enterUpgradeMode];
    };
}

-(void)chooseBtn:(UIButton *)button {
    if([RFIDBlutoothManager shareManager].isgetLab) {
        if(button.tag != 0){
            [self.scanView.stopBtn sendActionsForControlEvents:UIControlEventTouchUpInside];
            //[BSprogreUtil showMBProgressWith:self.view andTipString:@"Please stop scan first" autoHide:YES];
        }
        //return;
    }
    
    self.chooseIndex = button.tag;
    
    for (UIView *linView in self.dataLinView) {
        linView.hidden=YES;
    }
    UIView *linView=self.dataLinView[button.tag];
    linView.hidden=NO;
    
    for (UIView *view in self.dataBottomView) {
        view.hidden=YES;
    }
    UIView *view=self.dataBottomView[button.tag];
    view.hidden=NO;
}

-(void)initModifyBluetoothNameView {
    self.modifyNameView = [[ModifyBluetoothNameView alloc]init];
    [self.view addSubview:self.modifyNameView];
    [self.modifyNameView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.bottom.equalTo(self.view);
        make.top.equalTo(self->_middleScrollView.mas_bottom);
    }];
    self.modifyNameView.tag = 10;
    self.modifyNameView.hidden = YES;
    [self.dataBottomView addObject:self.modifyNameView];
    [self.modifyNameView setClickSaveBtnBlock:^(UIButton * _Nonnull btn, NSString * _Nonnull newName) {
        if ([newName isEqualToString:@""] || !newName) {
            
        }
    }];
}

#pragma mark - 蓝牙状态变化 Bluetooth status change
// PeripheralAddDelegate
- (void)addPeripheralWithPeripheral:(BLEModel  *)peripheralModel {
}

//蓝牙连接失败
- (void)connectBluetoothFailWithMessage:(NSString *)msg {
}

// 蓝牙设备连接成功
- (void)fatscaleDidConnectSucess {
}

//连接外设成功
-(void)connectPeripheralSuccess:(NSString *)nameStr {
    NSLog(@"nameStr===%@",nameStr);
    self.topView.stateLab.text=[NSString stringWithFormat:@"Device: %@",nameStr];
    [self.topView.connectBtn setTitle:@"Disconnect" forState:UIControlStateNormal];
    self.isConnect=YES;
    [BSprogreUtil showMBProgressWith:self.view andTipString:@"Normal communication" autoHide:YES];
    [self.scanView connectState];
}

//断开外设
-(void)disConnectPeripheral {
    self.topView.stateLab.text=@"Device: Not Connected";
    [self.topView.connectBtn setTitle:@"Connect" forState:UIControlStateNormal];
    self.isConnect=NO;
    [BSprogreUtil showMBProgressWith:self.view andTipString:@"Bluetooth is disconnected" autoHide:YES];
    [self.scanView disConnectState];
}


#pragma mark - 成功接收到蓝牙设备数据 Successful reception of Bluetooth device data
-(void)receiveDataWithBLEmodel:(BLEModel *)model result:(NSString *)result {
    if ([result isEqualToString:@"0"]) {
        if (model!=nil) {
            [self.searchAlert.dataSource addObject:model];
            [self.searchAlert.myTableView reloadData];
        }
    } else {
        [self.searchAlert.cannelBtn setTitle:@"Scan" forState:UIControlStateNormal];
    }
    [self.searchAlert didSelectItem:^(BLEModel *model) {
        [[RFIDBlutoothManager shareManager] connectPeripheral:model.peripheral macAddress:model.addressStr];
    }];
}


#pragma mark - 标签数据回调 Rfid Tag CallBack
///设置过滤数据回调
- (void)rfidSetFilterCallback:(NSString *)dataStr isSuccess:(BOOL)flag {
    if(flag) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"success" autoHide:YES];
    } else {
        [BSprogreUtil showMBProgressWith:self.view andTipString:dataStr hideTime:2];
    }
}
///标签数据回调
- (void)rfidTagInfoCallback:(UHFTagInfo *)tag {
    Boolean isHave=false;
    for (NSInteger i = 0 ; i < self.scanView.dataSource.count; i ++) {
        UHFTagInfo *oldEPC = self.scanView.dataSource[i];
        if ([[oldEPC.epc stringByAppendingString:oldEPC.tid] isEqualToString:[tag.epc stringByAppendingString:tag.tid]]) {
            isHave = YES;
            oldEPC.count++;
            oldEPC.rssi = tag.rssi;
            oldEPC.tid = tag.tid;
            oldEPC.user = tag.user;
            break;
        }
    }
    if(!isHave){
        [self.scanView.dataSource addObject:tag];
    }
    self.scanView.allCount++;
    dispatch_async(dispatch_get_main_queue(), ^{
        [self.scanView.tableView reloadData];
        self.scanView.countLab.text=[NSString stringWithFormat:@"%lu",(unsigned long)self.scanView.dataSource.count];
        self.scanView.allCountLab.text=[NSString stringWithFormat:@"%ld", (long)self.scanView.allCount];
    });
}

#pragma mark - 二维条码扫描回调 Barcode CallBack
- (void)rfidBarcodeLabelCallBack:(NSData *)data {
    NSLog(@"二维数据回调：%@", data);
    if (data == nil) {
        return;
    }
    
    [self.codeView.dataSource addObject:data];
    dispatch_async(dispatch_get_main_queue(), ^{
        [self.codeView.tableView reloadData];
        //使tableView滚动到底部
        NSInteger lastRow = [self.codeView.tableView numberOfRowsInSection:0] - 1;
        NSIndexPath *lastIndexPath = [NSIndexPath indexPathForRow:lastRow inSection:0];
        [self.codeView.tableView scrollToRowAtIndexPath:lastIndexPath atScrollPosition:UITableViewScrollPositionBottom animated:YES];
    });
}

#pragma mark - 设置相关指令回调  Config Callback
- (void)rfidConfigCallback:(NSString *)data function:(int)function {
    // 获取硬件版本号
    if (function == 0x01) {
        NSString *str = [NSString stringWithFormat:@"Hardware Versions：%@", data];
        [BSprogreUtil showMBProgressWith:self.view andTipString:str hideTime:2];
    }
    // 获取固件版本号
    else if (function == 0x03) {
        NSString *str = [NSString stringWithFormat:@"RFID Firmware Versions：%@",data];
        //usleep(1000 * 500);
        [BSprogreUtil showMBProgressWith:self.view andTipString:str hideTime:2];
    }
    // 获取主板版本号
    else if (function == 0xc9) {
        NSString *str = [NSString stringWithFormat:@"Mainboard Version：%@",data];
        //usleep(1000 * 500);
        [BSprogreUtil showMBProgressWith:self.view andTipString:str hideTime:2];
    }
    // 设置频率
    else if (function == 0x2d) {
        NSString *msg = data.integerValue == 1 ? @"success" : @"fail";
        [BSprogreUtil showMBProgressWith:self.view andTipString:msg autoHide:YES];
    }
    // 获取频率   data 0-5 success
    else if (function == 0x2f) {
        //读取频率
        if (data.integerValue >= 0) {
            [self.settingview.workBtn setTitle:self.modelArr[data.integerValue] forState:UIControlStateNormal];
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"Read frequency successful" autoHide:YES];
        } else {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"fail" autoHide:YES];
        }
    }
    // 设置发射功率
    else if (function == 0x11) {
        NSString *msg = data.integerValue == 1 ? @"success" : @"fail";
        usleep(1000 * 500);
        [BSprogreUtil showMBProgressWith:self.view andTipString:msg autoHide:YES];
    }
    // 获取发射功率
    else if (function == 0x13) {
        [self.settingview.putBtn setTitle:data forState:UIControlStateNormal];
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"Read power successfully" autoHide:YES];
    }
    // 设置定频
    else if (function == 0x15) {
        NSString *msg = data.integerValue == 1 ? @"success" : @"fail";
        [BSprogreUtil showMBProgressWith:self.view andTipString:msg autoHide:YES];
    }
    // 设备温度   -1表示失败，其他表示成功
    else if (function==0x35) {
        NSString *msg = data.integerValue == -1 ? @"fail" : [NSString stringWithFormat:@"Temperature：%d%@", data.intValue, @"℃"];
        [BSprogreUtil showMBProgressWith:self.view andTipString:msg autoHide:YES];
    }
    // 设置盘点模式
    else if (function==0x71) {
        NSString *msg = data.integerValue == 1 ? @"success" : @"fail";
        [BSprogreUtil showMBProgressWith:self.view andTipString:msg autoHide:YES];
    }
    // 获取盘点模式, 请求错误返回空字符串，正确返回：“type userPtr userLen”，例：“0 0 6”
    else if (function==0x73) {
        if (data.length < 3) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"fail" autoHide:YES];
            return;
        }
        NSArray<NSString *> *res = [data componentsSeparatedByString:@" "];
        [self.settingview chooseSelectWith:res[0]];
        self.settingview.userText.text = res[1];
        self.settingview.userLengthText.text = res[2];
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"success" autoHide:YES];
    }
    // 升级模式第一步
    else if (function == 0xC1) {
        if (data.intValue != 1) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"Failure to enter update mode!" autoHide:YES];
            return;
        }
        // 升级模式第一步成功后，进行第二步开始升级
        [[RFIDBlutoothManager shareManager] startUpgrade];
    }
    // 升级模式第二步
    else if (function == 0xC3) {
        if (data.integerValue != 1) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"Failure to start upgrade!" autoHide:YES];
            return;
        }
        self.updateIndex=0;
        // 升级模式第二步成功后，进行第三步发送数据每次发送64个字节，_updateArr是升级数据，每个元素为64字节的升级数据包。
        [[RFIDBlutoothManager shareManager] sendUpgradeData:_updateArr[self.updateIndex]];//发送升级数据
    }
    // 发送升级数据
    else if (function == 0xC5) {
        if (data.integerValue!=1) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"发送升级数据失败!" autoHide:YES];
            return;
        }
        self.updateIndex++;
        NSLog(@"发送 self.updateIndex=%ld  count=%lu",(long)self.updateIndex,(unsigned long)self.updateArr.count); //
        if (self.updateIndex == self.updateArr.count) {
            NSLog(@"发送完成"); //
            [[RFIDBlutoothManager shareManager] stopUpgrade];//退出升级模式
        } else {
            [[RFIDBlutoothManager shareManager] sendUpgradeData:_updateArr[self.updateIndex]];//发送升级数据
        }
    }
    // 退出升级模式
    else if (function == 0xC7) {
        if (data.integerValue != 1) {
            [BSprogreUtil showMBProgressWith:self.view andTipString:@"升级失败!" autoHide:YES];
            return;
        }
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"升级成功" autoHide:YES];
        NSLog(@"升级成功");
    }
    // 获取主板版本号
    else if (function == 0xc9) {
        
    }
    // 获取电池电量   data: 0-100
    else if (function == 0xE5) {
        NSString *str = [NSString stringWithFormat:@"Battery：%@%@", data, @"%"];
        [BSprogreUtil showMBProgressWith:self.view andTipString:str autoHide:YES];
    }
    // 开启蜂鸣器   data  1:成功   0:失败
    else if (function == 0xE500) {
        NSString *msg = data.integerValue == 1 ? @"success" : @"fail";
        [BSprogreUtil showMBProgressWith:self.view andTipString:msg autoHide:YES];
    }
    // 关闭蜂鸣器   data  1:成功   0:失败
    else if (function == 0xE501) {
        NSString *msg = data.integerValue == 1 ? @"success" : @"fail";
        [BSprogreUtil showMBProgressWith:self.view andTipString:msg autoHide:YES];
    }
    // 按硬件scan按钮之后的返回
    else if (function == 0xE6) {
        NSInteger keyCode = data.integerValue;
        NSLog(@"keyCode=%ld", keyCode);
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(0.5 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
            if (self.chooseIndex == 0) {
                //当前New Inventory界面选中标签界面  开始进行标签扫描或者停止操作
                if(keyCode == 1 || keyCode == 2) {
                    if ([RFIDBlutoothManager shareManager].isgetLab) {
                        NSLog(@"1 keydown NewInventory stopscan");
                        [self.scanView.stopBtn sendActionsForControlEvents:UIControlEventTouchUpInside];
                    } else {
                        NSLog(@"1 keydown NewInventory scan");
                        [self.scanView.beginBtn sendActionsForControlEvents:UIControlEventTouchUpInside];
                    }
                } else if(keyCode == 3) {
                    [self.scanView.beginBtn sendActionsForControlEvents:UIControlEventTouchUpInside];
                } else if(keyCode == 4) {
                    [self.scanView.stopBtn sendActionsForControlEvents:UIControlEventTouchUpInside];
                }
            } else if (self.chooseIndex == 1) {
                //当前界面选中的是二维码扫描  开始进行二维码扫描
                if(keyCode == 1 || keyCode == 2){
                    [[RFIDBlutoothManager shareManager] start2DScan];
                } else if(keyCode == 3) {
                    [[RFIDBlutoothManager shareManager] start2DScan];
                } else if(keyCode == 4) {
                    //[[RFIDBlutoothManager shareManager] stopContinuity2DScan];
                }
            }
        });
    }
    // 设置密钥
    else if (function == 0xe31) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:data autoHide:YES];
    }
    // 获取密钥
    else if (function == 0xe32) {
        self.encryView.textField1.text=[data substringWithRange:NSMakeRange(0, 32)];
        self.encryView.textField2.text=[data substringWithRange:NSMakeRange(32, 32)];
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"Get Key Success" autoHide:YES];
    }
    // SM4加密
    else if (function == 0xe33) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"Encryption success" autoHide:YES];
        self.encryView.textField3.text=data;
    }
    // SM4解密
    else if (function == 0xe34) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"Decryption success" autoHide:YES];
        self.encryView.textField4.text=data;
    }
    // USER加密
    else if (function == 0xe35){
        [BSprogreUtil showMBProgressWith:self.view andTipString:data autoHide:YES];
    } 
    // USER解密
    else if (function == 0xe36) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"read success" autoHide:YES];
        self.userrView.readField.text=data;
    }
    
}

#pragma mark - 读、写、锁、销毁数据回调  Read/Write/Lock/Kill Tag Callback
// 读标签数据回调
- (void)rfidReadLabelCallback:(NSString *)dataStr isSuccess:(BOOL)flag {
    if (flag) {
        self.readView.dataField1.text = dataStr;
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"success" autoHide:YES];
    } else {
        self.readView.dataField1.text = @"";
        [BSprogreUtil showMBProgressWith:self.view andTipString:dataStr autoHide:YES];
    }
}
// 写标签数据回调
- (void)rfidWriteLabelCallback:(NSString *)dataStr isSuccess:(BOOL)flag {
    if (flag) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"success" autoHide:YES];
    } else {
        [BSprogreUtil showMBProgressWith:self.view andTipString:dataStr autoHide:YES];
    }
}
// 锁标签数据回调
- (void)rfidLockLabelCallback:(NSString *)dataStr isSuccess:(BOOL)flag {
    if (flag) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"success" autoHide:YES];
    } else {
        [BSprogreUtil showMBProgressWith:self.view andTipString:dataStr autoHide:YES];
    }
}
// 销毁标签数据回调
- (void)rfidKillLabelCallback:(NSString *)dataStr isSuccess:(BOOL)flag {
    if (flag) {
        [BSprogreUtil showMBProgressWith:self.view andTipString:@"success" autoHide:YES];
    } else {
        [BSprogreUtil showMBProgressWith:self.view andTipString:dataStr autoHide:YES];
    }
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
